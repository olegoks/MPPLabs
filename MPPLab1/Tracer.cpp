
#include <rapidjson/document.h>
#include <rapidjson/stringbuffer.h>
#include <rapidjson/prettywriter.h>
#include <rapidxml.hpp>
#include <rapidxml_print.hpp>
#include <mutex>

#include "Tracer.hpp"

ITracer::Structure::Function::Function(__STD weak_ptr<Function>& caller) noexcept :
	start_info_{ },
	stop_info_{ },
	child_functions_{ },
	caller_{ caller }{}

ITracer::Structure::Function::Function(Function&& move_func) noexcept :
	start_info_{ __STD move(move_func.start_info_) },
	stop_info_{ __STD move(move_func.stop_info_) },
	child_functions_{ __STD move(move_func.child_functions_) },
	caller_{ __STD move(move_func.caller_) }{}

ITracer::Structure::Function::Function(const Function& copy_func) noexcept :
	start_info_{ copy_func.start_info_ },
	stop_info_{ copy_func.stop_info_ },
	child_functions_{ copy_func.child_functions_ },
	caller_{ copy_func.caller_ }{}

__STD weak_ptr<ITracer::Structure::Function> ITracer::Structure::Function::GetLastFunction() noexcept {

	return child_functions_.back();

}

__STD string_view ITracer::Structure::Function::GetName() const noexcept {

	return start_info_.GetFunctionName();

}
bool ITracer::Structure::Function::IsMethod() const noexcept {

	return start_info_.GetClassName().size();

}
__STD string_view ITracer::Structure::Function::GetClassName() const noexcept {

	return start_info_.GetClassName();

}
__STD shared_ptr<ITracer::Structure::Function> ITracer::Structure::Function::operator[](__STD uint64_t index) const noexcept {

	return child_functions_[index];

}
ITracer::Structure::Function& ITracer::Structure::Function::operator=(Function&& func) noexcept {

	if (this == &func)return *this;

	start_info_ = __STD move(func.start_info_);
	stop_info_ = __STD move(func.stop_info_);
	child_functions_ = __STD move(func.child_functions_);
	caller_ = __STD move(func.caller_);


	return *this;

}
__STD uint64_t ITracer::Structure::Function::GetExecuteTimeMs() const noexcept {

	return __STD chrono::duration_cast<__STD chrono::milliseconds>(stop_info_.GetTimePoint() - start_info_.GetTimePoint()).count();

}
void ITracer::Structure::Function::SetStartInfo(const StartInfo& start) noexcept {

	start_info_ = start;

}
void ITracer::Structure::Function::SetStopInfo(const StopInfo& stop) noexcept {

	stop_info_ = stop;

}
void ITracer::Structure::Function::AddFunction(__STD shared_ptr<Function>& func) noexcept {

	child_functions_.push_back(func);

}
__STD uint64_t ITracer::Structure::Function::GetChildFunctionsNumber() const noexcept {

	return child_functions_.size();

}
__STD weak_ptr<ITracer::Structure::Function> ITracer::Structure::Function::GetCaller() noexcept {

	return caller_;

}
ITracer::StartInfo::StartInfo(time_point start, thread_id thread_id, __STD string_view class_name, __STD string_view name) noexcept :
	start_point_{ start },
	thread_id_{ thread_id },
	class_{ class_name },
	name_{ name }{}

ITracer::time_point ITracer::StartInfo::GetTimePoint() const noexcept {

	return start_point_;

}

__STD string_view ITracer::StartInfo::GetFunctionName() const noexcept {

	return name_;

}

__STD string_view ITracer::StartInfo::GetClassName() const noexcept {

	return class_;

}

ITracer::StopInfo::StopInfo(time_point end, thread_id thread_id) noexcept :
	end_point_{ end },
	thread_id_{ thread_id }{}

bool ITracer::StopInfo::operator!=(StopInfo& compare_info) noexcept {

	return (end_point_ != compare_info.end_point_) || (thread_id_ != compare_info.thread_id_);

}

ITracer::time_point ITracer::StopInfo::GetTimePoint() const noexcept {

	return end_point_;

}

ITracer::Structure::Thread::Thread() noexcept :
	id_{ },
	entry_point_{},
	current_function_{ entry_point_ }{}

ITracer::Structure::Thread::Thread(thread_id id) noexcept :
	id_{ id },
	entry_point_{},
	current_function_{ entry_point_ }{}

void ITracer::Structure::Thread::AddFunctionStartInfo(const StartInfo& start_info) {

	auto new_func = __STD make_shared<Function>(current_function_);
	new_func->SetStartInfo(start_info);
	if (entry_point_ == nullptr) {
		entry_point_ = new_func;
		current_function_ = entry_point_;
	}
	else {
		current_function_.lock()->AddFunction(new_func);
		current_function_ = current_function_.lock()->GetLastFunction();
	}

}

void ITracer::Structure::Thread::AddFunctionStopInfo(const StopInfo& info) {

	current_function_.lock()->SetStopInfo(info);
	current_function_ = current_function_.lock()->GetCaller();

}

void ITracer::Structure::AddFunctionStartInfo(thread_id id, const StartInfo& func) {

	if (threads_.find(id) == threads_.end()) {
		Thread new_thread{ id };
		threads_[id] = __STD move(new_thread);
	}
	Thread& thread = threads_[id];
	thread.AddFunctionStartInfo(func);

}

void ITracer::Structure::AddFunctionStopInfo(thread_id id, const StopInfo& info) {

	Thread& thread = threads_[id];
	thread.AddFunctionStopInfo(info);

}

void ITracer::Structure::ForEachThread(__STD function<void(const Thread&)> callback) const noexcept {

	for (const auto& [thread_id, thread] : threads_)
		callback(thread);

}

ITracer::JSON::JSON(const __STD string& text) noexcept :
	text_{ text } {}

const __STD string& ITracer::JSON::GetString() const noexcept {

	return text_;

}

using JSONAlloc = rapidjson::MemoryPoolAllocator<rapidjson::CrtAllocator>;


void JSONAddFunction(rapidjson::Value& array, JSONAlloc& alloc, const ITracer::Struct::Function& func) {

	using namespace rapidjson;
	assert(array.IsArray());
	Value func_obj;
	func_obj.SetObject();
	func_obj.AddMember("name", Value{}.SetString(func.GetName().data(), func.GetName().length()), alloc);
	func_obj.AddMember("time", Value{}.SetUint64(func.GetExecuteTimeMs()), alloc);
	func_obj.AddMember("functions", Value{}.SetArray(), alloc);
	array.PushBack(__STD move(func_obj), alloc);
	for (__STD uint64_t i = 0; i < func.GetChildFunctionsNumber(); ++i) {
		JSONAddFunction(array[array.Size() - 1]["functions"], alloc, *func[i]);
	}

}

void XMLAddFunction(rapidxml::xml_node<>* array, rapidxml::xml_document<>& doc, const ITracer::Struct::Function& func) {

	using namespace rapidxml;
	xml_node<>* func_node = doc.allocate_node(node_element, "function");
	func_node->append_attribute(doc.allocate_attribute("class", func.GetClassName().data()));
	func_node->append_attribute(doc.allocate_attribute("name", func.GetName().data()));
	func_node->append_attribute(doc.allocate_attribute("time", doc.allocate_string(__STD to_string(func.GetExecuteTimeMs()).c_str())));
	array->append_node(func_node);
	for (__STD uint64_t i = 0; i < func.GetChildFunctionsNumber(); ++i) {
		XMLAddFunction(func_node, doc, *func[i]);
	}

}

ITracer::Result::Result(Structure&& structure) noexcept :
	struct_{ __STD move(structure) } {}

ITracer::Result::Result(const Structure& structure) noexcept :
	struct_{ structure } {}


ITracer::JSON ITracer::Result::ConvertToJSON() const noexcept {

	using namespace rapidjson;
	Document doc;
	auto& alloc = doc.GetAllocator();
	doc.SetObject().AddMember("threads", Value{}.SetArray(), alloc);
	Value& threads_array = doc["threads"];
	struct_.ForEachThread([&](const Struct::Thread& thread)noexcept->void {

		Value thread_info;
		thread_info.SetObject();
		__STD ostringstream ss;
		ss << thread.id_;
		__STD uint64_t id{ __STD stoull(ss.str()) };
		thread_info.AddMember("id", Value{}.SetUint64(id), alloc);
		thread_info.AddMember("time, ms", Value{}.SetUint64(thread.entry_point_->GetExecuteTimeMs()), alloc);
		thread_info.AddMember("functions", Value{}.SetArray(), alloc);
		JSONAddFunction(thread_info["functions"], alloc, *thread.entry_point_);
		threads_array.PushBack(__STD move(thread_info), alloc);

		});

	StringBuffer buffer;
	PrettyWriter<StringBuffer> writer(buffer);
	doc.Accept(writer);

	return JSON{ buffer.GetString() };
}

ITracer::XML ITracer::Result::ConvertToXML() const noexcept {

	using namespace rapidxml;
	xml_document<> doc;
	xml_node<>* root = doc.allocate_node(node_element, "root");
	doc.append_node(root);

	struct_.ForEachThread([&](const Struct::Thread& thread)noexcept->void {

		xml_node<>* thread_node = doc.allocate_node(node_element, "thread");
		__STD ostringstream ss;
		ss << thread.id_;
		__STD string id = ss.str();
		__STD string time = __STD to_string(thread.entry_point_->GetExecuteTimeMs()).c_str();
		thread_node->append_attribute(doc.allocate_attribute("id", doc.allocate_string(id.c_str())));
		thread_node->append_attribute(doc.allocate_attribute("time, ms", doc.allocate_string(time.c_str())));
		root->append_node(thread_node);
		XMLAddFunction(thread_node, doc, *thread.entry_point_);

		});

	std::string xml_as_string;
	rapidxml::print(std::back_inserter(xml_as_string), doc);
	return XML{ xml_as_string };

}

void Tracer::StartTrace(__STD string_view class_name, __STD string_view method_name) {

	static __STD mutex mtx;
	__STD lock_guard<__STD mutex> lock{ mtx };
	const thread_id current_thread_id = __STD this_thread::get_id();
	StartInfo start_info{ clock::now(), __STD this_thread::get_id(), class_name, method_name };
	struct_.AddFunctionStartInfo(current_thread_id, start_info);

}

void Tracer::StopTrace() {

	static __STD mutex mtx;
	__STD lock_guard<__STD mutex> lock{ mtx };
	StopInfo stop_info{ clock::now(), __STD this_thread::get_id() };
	const thread_id current_thread_id = __STD this_thread::get_id();
	struct_.AddFunctionStopInfo(current_thread_id, stop_info);

}

ITracer::Result Tracer::GetResult() {
	return Result{ struct_ };
}

const __STD string& ITracer::XML::GetString() const noexcept {

	return text_;

}

ITracer::Guard::Guard(__STD shared_ptr<ITracer> tracer, __STD string_view class_name, __STD string_view method_name) noexcept :
	tracer_{ tracer } {

	tracer_->StartTrace(class_name, method_name);

}

ITracer::Guard::~Guard() noexcept {

	tracer_->StopTrace();

}

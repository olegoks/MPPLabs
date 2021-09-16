
#include <rapidjson/document.h>
#include <rapidjson/stringbuffer.h>
#include <rapidjson/prettywriter.h>
#include <rapidxml.hpp>
#include <rapidxml_print.hpp>
#include <mutex>

#include "Tracer.hpp"

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



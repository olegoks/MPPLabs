#pragma once

#include <iostream>
#include <stack>
#include <vector>
#include <chrono>
#include <string>
#include <string_view>
#include <thread>

#include <memory>
#include <typeinfo>
#include <functional>
#include <fstream>
#include <map>
#include <iostream>
#include <sstream>


#define __INTERFACE
#define __STD ::std::

__INTERFACE class ITracer {
public:

	using time_point = __STD chrono::high_resolution_clock::time_point;
	using duration = __STD chrono::high_resolution_clock::duration;
	using clock = __STD chrono::high_resolution_clock;
	using thread_id = __STD thread::id;

protected:

	class StartInfo final {
	private:

		time_point	start_point_;
		thread_id	thread_id_;
		__STD string_view class_;
		__STD string_view name_;

	public:

		explicit StartInfo()noexcept = default;
		explicit StartInfo(time_point start, thread_id thread_id, __STD string_view class_name, __STD string_view name)noexcept;
		StartInfo& operator=(const StartInfo& copy_info)noexcept = default;
		time_point GetTimePoint()const noexcept;
		__STD string_view GetFunctionName()const noexcept;
		__STD string_view GetClassName()const noexcept;

	};
	class StopInfo final {
	private:

		time_point	end_point_;
		thread_id	thread_id_;

	public:

		explicit StopInfo(time_point end, thread_id thread_id)noexcept;
		explicit StopInfo()noexcept = default;
		StopInfo& operator=(const StopInfo& copy_info)noexcept = default;
		bool operator!=(StopInfo& compare_info)noexcept;
		time_point GetTimePoint()const noexcept;

	};

public:

	class Structure final {
	public:

		class Function {
		private:

			using Functions = __STD vector<__STD shared_ptr<Function>>;

			StartInfo	start_info_;
			StopInfo	stop_info_;
			Functions	child_functions_;
			__STD weak_ptr<Function> caller_;

		public:

			Function()noexcept = default;
			Function(__STD weak_ptr<Function>& caller)noexcept;
			Function(Function&& move_func)noexcept;
			Function(const Function& copy_func)noexcept;
			__STD weak_ptr<Function> GetLastFunction()noexcept;
			__STD string_view GetName()const noexcept;
			bool IsMethod()const noexcept;
			__STD string_view GetClassName()const noexcept;
			__STD shared_ptr<Function> operator[](__STD uint64_t index)const noexcept;
			Function& operator=(Function&& func)noexcept;
			Function& operator=(const Function& func)noexcept {

				if (this == &func)return *this;

				start_info_ = func.start_info_;
				stop_info_ = func.stop_info_;
				child_functions_ = func.child_functions_;
				caller_ = func.caller_;

				return *this;

			}
			__STD uint64_t GetExecuteTimeMs()const noexcept;
			void SetStartInfo(const StartInfo& start)noexcept;
			void SetStopInfo(const StopInfo& stop)noexcept;
			void AddFunction(__STD shared_ptr<Function>& func)noexcept;
			__STD uint64_t GetChildFunctionsNumber()const noexcept;
			__STD weak_ptr<Function> GetCaller()noexcept;
		};

		struct Thread final {

			thread_id id_;
			__STD shared_ptr<Function>	entry_point_;
			__STD weak_ptr<Function>	current_function_;

			Thread()noexcept;
			Thread(thread_id id)noexcept;

			void AddFunctionStartInfo(const StartInfo& start_info);
			void AddFunctionStopInfo(const StopInfo& info);

		};

	private:

		__STD map<thread_id, Thread> threads_;

	public:

		Structure()noexcept :
			threads_{} {}

		Structure(const Structure& structure)noexcept :
			threads_{ structure.threads_ } {}

		void AddFunctionStartInfo(thread_id id, const StartInfo& func);
		void AddFunctionStopInfo(thread_id id, const StopInfo& info);
		void ForEachThread(__STD function<void(const Thread&)> callback)const noexcept;
	};
	using Struct = Structure;
	class IResultFormat {
	public:

		virtual const __STD string& GetString()const noexcept = 0;
		friend __STD ostream& operator<<(__STD ostream& os, const IResultFormat& format)noexcept;
		friend __STD ofstream& operator<<(__STD ofstream& os, const IResultFormat& format)noexcept;

	};
	class JSON final : public IResultFormat {
	private:

		__STD string text_;

	public:

		explicit JSON(const __STD string& text)noexcept;
		virtual const __STD string& GetString()const noexcept override;

	};
	class XML final : public IResultFormat {
	private:

		__STD string text_;

	public:

		explicit XML(const __STD string& text)noexcept :
			text_{ text } {}

		virtual const __STD string& GetString()const noexcept override;

	};
	class Result {
	public:

		using Callback = __STD function<void(const Structure::Function&)>;
		
	private:

		Structure struct_;
		

	public:

		explicit Result(Structure&& structure)noexcept;
		explicit Result(const Structure& structure)noexcept;

		JSON ConvertToJSON()const noexcept;
		XML ConvertToXML()const noexcept;

	};
	class Guard final {
	private:

		__STD shared_ptr<ITracer> tracer_;

	public:

		explicit Guard(__STD shared_ptr<ITracer> tracer, __STD string_view class_name, __STD string_view method_name)noexcept;
		~Guard()noexcept;

	};
	virtual void StartTrace(__STD string_view class_name, __STD string_view method_name) = 0;
	virtual void StopTrace() = 0;
	virtual Result GetResult() = 0;

};
inline __STD ostream& operator<<(__STD ostream& os, const ITracer::IResultFormat& format)noexcept {

	os << __STD endl << format.GetString() << __STD endl;
	return os;

}
inline __STD ofstream& operator<<(__STD ofstream& fs, const ITracer::IResultFormat& format)noexcept {

	fs << format.GetString().c_str() << __STD endl;
	return fs;

}

#define TRACE_METHOD(tracer)ITracer::Guard guard{ tracer, typeid(*this).name(), __func__ };
#define TRACE_FUNCTION(tracer)ITracer::Guard guard{ tracer, "", __func__ };

class Tracer final : public ITracer {
private:

	Structure struct_;

public:

	explicit Tracer()noexcept :
		struct_{} {}
	virtual void StartTrace(__STD string_view class_name, __STD string_view method_name)override;
	virtual void StopTrace()override;
	virtual Result GetResult()override;

};
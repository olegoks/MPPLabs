#include <Tracer.hpp>
class Bar {
private:

	__STD shared_ptr<ITracer> tracer_;

public:

	explicit Bar(__STD shared_ptr<ITracer> tracer)noexcept :
		tracer_{ tracer } {

		//TRACE_FUNCTION(tracer);

	}

	void Test2Bar() {

		TRACE_METHOD(tracer_);
		using namespace __STD chrono_literals;
		__STD this_thread::sleep_for(10ms);
	}
	void TestBar()noexcept {

		TRACE_METHOD(tracer_);

		using namespace __STD chrono_literals;
		__STD this_thread::sleep_for(20ms);

		Test2Bar();

	}

	~Bar()noexcept = default;

};
class Foo {
private:

	__STD shared_ptr<ITracer> tracer_;
	Bar bar_;

public:

	explicit Foo(__STD shared_ptr<ITracer> tracer)noexcept :
		tracer_{ tracer },
		bar_{ tracer_ }{

	}

	void TestFoo() {

		TRACE_METHOD(tracer_);

		bar_.TestBar();
		using namespace __STD chrono_literals;
		__STD this_thread::sleep_for(30ms);
		bar_.TestBar();


	}

};

__STD uint64_t SubStringsNumber(const __STD string& str, const __STD string sub_str) {

	int occurrences = 0;
	std::string::size_type pos = 0;
	while ((pos = str.find(sub_str, pos)) != std::string::npos) {
		++occurrences;
		pos += sub_str.length();
	}
	return occurrences;

}

void MultiThreadTest(__STD shared_ptr<ITracer> tracer_, Foo* foo) {

	TRACE_FUNCTION(tracer_);

	using namespace __STD chrono_literals;
	__STD this_thread::sleep_for(30ms);
	foo->TestFoo();
}

int main() {
	//auto tracer = __STD make_shared<Tracer>();
	//{
	//	TRACE_FUNCTION(tracer);
	//	Foo foo{ tracer };
	//	foo.TestFoo();
	//	foo.TestFoo();

	//	//__STD thread thrd{ MultiThreadTest, tracer, &foo };
	//	//thrd.join();
	//}

	//using namespace __STD string_literals;
	//ITracer::Result result = tracer->GetResult();
	//__STD cout << result.ConvertToJSON().GetString() << std::endl;
	//__STD cout << result.ConvertToXML().GetString() << std::endl;


	auto tracer = __STD make_shared<Tracer>();
	{TRACE_FUNCTION(tracer);

	//Foo foo{ tracer };
	//foo.TestFoo();
	//foo.TestFoo();

	//__STD thread thrd1{ MultiThreadTest, tracer, &foo };
	//__STD thread thrd2{ MultiThreadTest, tracer, &foo };
	//__STD thread thrd3{ MultiThreadTest, tracer, &foo };
	//__STD thread thrd4{ MultiThreadTest, tracer, &foo };
	using namespace __STD chrono_literals;
	__STD this_thread::sleep_for(1s);
	//thrd1.join();
	//thrd2.join();
	//thrd3.join();
	//thrd4.join();
	}
	using namespace __STD string_literals;
	ITracer::Result result = tracer->GetResult();
	__STD cout << result.ConvertToJSON().GetString();
	__STD cout << result.ConvertToXML().GetString();
	return 0;
}
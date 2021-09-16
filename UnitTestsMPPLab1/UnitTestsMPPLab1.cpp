#include "pch.h"
#include "CppUnitTest.h"
#include "../MPPLab1/Tracer.hpp"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace UnitTestsMPPLab1
{
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

	void MultiThreadTest(__STD shared_ptr<ITracer> tracer_, Foo* foo) {

		TRACE_FUNCTION(tracer_);

		using namespace __STD chrono_literals;
		__STD this_thread::sleep_for(30ms);
		foo->TestFoo();
	}

	__STD uint64_t SubStringsNumber(const __STD string& str, const __STD string sub_str) {

		int occurrences = 0;
		std::string::size_type pos = 0;
		while ((pos = str.find(sub_str, pos)) != std::string::npos) {
			++occurrences;
			pos += sub_str.length();
		}
		return occurrences;

	}


	TEST_CLASS(UnitTestsMPPLab1)
	{
	public:

		TEST_METHOD(EmptyTracerGetResult) {

			Tracer tracer;
			using namespace __STD string_literals;
			ITracer::Result result = tracer.GetResult();
			Assert::AreEqual(result.ConvertToJSON().GetString(), "{\n    \"threads\": []\n}"s);
			Assert::AreEqual(result.ConvertToXML().GetString(), "<root/>\n\n"s);

		}
		TEST_METHOD(TraceOneFunction) {

			auto tracer = __STD make_shared<Tracer>();
			TRACE_FUNCTION(tracer);
			using namespace __STD string_literals;
			ITracer::Result result = tracer->GetResult();
			Assert::IsTrue(SubStringsNumber(result.ConvertToJSON().GetString(), "\"name\"") == 1);
			Assert::IsTrue(SubStringsNumber(result.ConvertToXML().GetString(), "<function") == 1);

		}

		TEST_METHOD(TraceFunctionsAndMethods) {

			auto tracer = __STD make_shared<Tracer>();
			{
				TRACE_FUNCTION(tracer);

			Foo foo{ tracer };
			foo.TestFoo();
			foo.TestFoo();
			}
			using namespace __STD string_literals;
			ITracer::Result result = tracer->GetResult();
			Assert::IsTrue(SubStringsNumber(result.ConvertToJSON().GetString(), "\"name\"") == 11);
			Assert::IsTrue(SubStringsNumber(result.ConvertToXML().GetString(), "<function") == 11);

		}

		TEST_METHOD(TraceTwoThreads) {

			auto tracer = __STD make_shared<Tracer>();
			{TRACE_FUNCTION(tracer);

			Foo foo{ tracer };
			foo.TestFoo();
			foo.TestFoo();

			__STD thread thrd{ MultiThreadTest, tracer, &foo };
			thrd.join();
			}
			using namespace __STD string_literals;
			ITracer::Result result = tracer->GetResult();
			Assert::IsTrue(SubStringsNumber(result.ConvertToJSON().GetString(), "\"name\"") == 17);
			Assert::IsTrue(SubStringsNumber(result.ConvertToJSON().GetString(), "\"id\"") == 2);
			Assert::IsTrue(SubStringsNumber(result.ConvertToXML().GetString(), "<function") == 17);
			Assert::IsTrue(SubStringsNumber(result.ConvertToXML().GetString(), "<thread") == 2);

		}

		TEST_METHOD(TraceFiveThreads) {

			auto tracer = __STD make_shared<Tracer>();
			TRACE_FUNCTION(tracer);

			Foo foo{ tracer };
			foo.TestFoo();
			foo.TestFoo();

			__STD thread thrd1{ MultiThreadTest, tracer, &foo };
			__STD thread thrd2{ MultiThreadTest, tracer, &foo };
			__STD thread thrd3{ MultiThreadTest, tracer, &foo };
			__STD thread thrd4{ MultiThreadTest, tracer, &foo };

			thrd1.join();
			thrd2.join();
			thrd3.join();
			thrd4.join();

			using namespace __STD string_literals;
			ITracer::Result result = tracer->GetResult();
			Assert::IsTrue(SubStringsNumber(result.ConvertToJSON().GetString(), "\"name\"") == 11 + 6 * 4);
			Assert::IsTrue(SubStringsNumber(result.ConvertToJSON().GetString(), "\"id\"") == 5);
			Assert::IsTrue(SubStringsNumber(result.ConvertToXML().GetString(), "<function") == 11 + 6 * 4);
			Assert::IsTrue(SubStringsNumber(result.ConvertToXML().GetString(), "<thread") == 5);

		}

	};
}

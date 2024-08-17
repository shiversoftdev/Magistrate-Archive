#ifndef TIMER_H
#define TIMER_H
#include <functional>
#include <chrono>
#include <future>
#include <cstdio>
class later {
public:
    template <class callable, class... arguments>
    later(int after, bool async, callable&& f, arguments&&... args) {
        std::function<typename std::result_of<callable(arguments...)>::type()> task(std::bind(std::forward<callable>(f), std::forward<arguments>(args)...));
        if (async) {
            std::thread([after, task]() {
                std::this_thread::sleep_for(std::chrono::milliseconds(after));
                task();
                }).detach();
        }
        else {
            std::this_thread::sleep_for(std::chrono::milliseconds(after));
            task();
        }
    }
};

#endif // TIMER_H
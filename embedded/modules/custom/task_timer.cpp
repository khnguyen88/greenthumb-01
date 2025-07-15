#include <chrono>

class Timer {
    // Properties
    public:
        unsigned long startTimeMillis;
        unsigned long maxPeriodTimeMillis;
        bool isItTaskTime = false;

    private:
        unsigned long currentTimeMillis;
        unsigned long currentDuration;

        std::chrono::time_point<std::chrono::steady_clock> startTimeChrono;
        std::chrono::time_point<std::chrono::steady_clock> currentTimeChrono;

    // Constructor
    private:
        // Default Constructor
        Timer() :
            currentTimeMillis(0), 
            maxPeriodTimeMillis(1000), //default at 1 sec
            startTimeChrono(std::chrono::steady_clock::now()),
            currentTimeChrono(std::chrono::steady_clock::now())
        {
            startTimeMillis = startTimeChrono.time_since_epoch().count();
            currentTimeMillis = currentTimeChrono.time_since_epoch().count();
        }

    public:
        // Public Delegating Constructor
        Timer(unsigned long maxPerMillis):
            startTimeChrono(std::chrono::steady_clock::now()),
            currentTimeChrono(std::chrono::steady_clock::now())
        {
            maxPeriodTimeMillis = maxPerMillis;
            startTimeMillis = startTimeChrono.time_since_epoch().count();
            currentTimeMillis = currentTimeChrono.time_since_epoch().count();
        }

    // Methods
    public:
        void setMaxPeriod(unsigned long maxPerMillis) {
            maxPeriodTimeMillis = maxPerMillis;
        }

        bool checkisItTaskTime(){
            bool condition = (currentTimeMillis - startTimeMillis >= maxPeriodTimeMillis);

            refreshTimer(condition);

            return condition;
        }

    private:
        void setCurrentTime(){
            currentTimeChrono = std::chrono::steady_clock::now();
            currentTimeMillis = currentTimeChrono.time_since_epoch().count();
        }

        void refreshTimer(bool isPassedDuration){
            if(isPassedDuration){
                startTimeMillis = currentTimeMillis;
            }

            setCurrentTime();
        }
};

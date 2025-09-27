#include "unit_converter.h"
//Const Variable
//========================================
// Time
// -----
const int hrsInDay = 24;            // 1 Day = 24 hrs
const int minInHour = 60;           // 1 Hr =  60 hrs
const int secPerMin = 60;        // 1 Min = 60 Sec
const int millisPerSec = 1000;      // 1 Sec = 1000 Millisec

// Volume
// -----
const int cubicInPerLiter = 61.024; // 1 L = 61.024 in^3


//# Methods
//========================================
// Time
// -----
//Long to Long
long secondsToMillis(long seconds){
  return seconds * millisPerSec;
}

long minutesToMillis(long minutes){
  return minutes * secPerMin * millisPerSec;
}

long hoursToMillis(long hours){
  return hours * minInHour * secPerMin * millisPerSec;
}

long millisToSeconds(long millis){
  return millis / millisPerSec;
}

long millisToMinutes(long millis){
  return millis / (secPerMin * millisPerSec);
}

long millisToHours(long millis){
  return millis / (minInHour * secPerMin * millisPerSec);
}

// Float to Long
long secondsToMillis(float seconds){
  return (long)(seconds * millisPerSec);
}

long minutesToMillis(float minutes){
  return (long)(minutes * secPerMin * millisPerSec);
}

long hoursToMillis(float hours){
  return (long)(hours * minInHour * secPerMin * millisPerSec);
}

long millisToSeconds(float millis){
  return (long)(millis / millisPerSec);
}

long millisToMinutes(float millis){
  return (long)(millis / (minInHour * millisPerSec));
}

long millisToHours(float millis){
  return (long)(millis / (minInHour * secPerMin * millisPerSec));
}

// Int to Long
long secondsToMillis(int seconds){
  return (long)(seconds * millisPerSec);
}

long minutesToMillis(int minutes){
  return (long)(minutes * secPerMin * millisPerSec);
}

long hoursToMillis(int hours){
  return (long)(hours * minInHour * secPerMin * millisPerSec);
}

long millisToSeconds(int millis){
  return (long)(millis / millisPerSec);
}

long millisToMinutes(int millis){
  return (long)(millis / (secPerMin * millisPerSec));
}

long millisToHours(int millis){
  return (long)(millis / (minInHour * secPerMin * millisPerSec));
}


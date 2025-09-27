#ifndef UNIT_CONVERTER_H
#define UNIT_CONVERTER_H

//# Methods
//========================================
// Time
// -----
//Long to Long
long secondsToMillis(long seconds);

long minutesToMillis(long minutes);

long hoursToMillis(long hours);

long millisToSeconds(long millis);

long millisToMinutes(long millis);

long millisToHours(long millis);

//Float to Long
long secondsToMillis(float seconds);

long minutesToMillis(float minutes);

long hoursToMillis(float hours);

long millisToSeconds(float millis);

long millisToMinutes(float millis);

long millisToHours(float millis);

//Int to Long
long secondsToMillis(int seconds);

long minutesToMillis(int minutes);

long hoursToMillis(int hours);

long millisToSeconds(int millis);

long millisToMinutes(int millis);

long millisToHours(int millis);

//# Const Variables
//========================================
// Time
// -----
extern const int hrsInDay;
extern const int minInHour;
extern const int secPerMin;
extern const int millisPerSec;

// Volume
// -----
extern const int cubicInPerLiter;
#endif
mergeInto(LibraryManager.library, {
  GetWeatherInLocation: function(weatherString)
  {
    var jsString = UTF8ToString(weatherString);
    getResults(jsString);
  },

  GetWeatherResult: function()
  {
    var returnStr = returnWeatherType();
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  GetTimeDecimal: function()
  {
    return returnCurrentTimeAsDecimal();
  },

  GetTimeReal: function()
  {
    var returnStr = returnTimeAndDate();
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  GetWeatherDescription: function()
  {
    return returnWeatherDescription();
  },

  GetLocationNameAsString: function()
  {
    var returnStr = returnLocationName();
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  GetTemperature: function()
  {
    return returnTemperature();
  }
});

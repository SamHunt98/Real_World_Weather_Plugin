const api ={
    key: "8ebe83726a399ffe773ab79abd7e15f3",
    baseurl: "https://api.openweathermap.org/data/2.5/"

}

var currentWeather;
var currentTimeInLocation;
//setDefault();
function setDefault()
{
	getResults("London,GB");
}

function getResults(query)
{
    fetch(`${api.baseurl}weather?q=${query}&units=metric&APPID=${api.key}`).then(weather =>{
        return weather.json();
    }).then(startChain);
}
function startChain(w)
{
    console.log("chain started");
    if(w.cod != 404) 
    {
        currentWeather = w;
    }
    else
    {
        //if the API does not find a location it will not return a weather value, just a code of 404. 
        //Checking for this allows us to keep the program from crashing by keeping the weather unchanged from the last working version
        console.log("This query is not valid");
    }
}

function returnLocationName()
{
    
    return `${currentWeather?.name}, ${currentWeather?.sys?.country}`;

}
function returnWeatherType()
{

    return currentWeather?.weather[0]?.main;

}

function returnWeatherDescription()
{
    //returns the ID of the weather type, which will allow for differentiation between weather of the same main type
    //for example, id's 201 and 210 both refer to a thunderstorm, but the first is a storm with rain while the second is dry

    return currentWeather?.weather[0]?.id;


}
function convertTimeZone()
{
    //converts the UTC time output by the API to the current time in the specified location
    d = new Date();
    localTime = d.getTime();
    localOffset = d.getTimezoneOffset() * 60000; //local timezone offset converted to MS
    utc = localTime + localOffset; //uses the local timezone offset to convert local time to UTC
    var conv = utc + (1000 * currentWeather?.timezone); //adds returned weather's timezone offset as MS
    currentTimeInLocation = new Date(conv); //this should give the current time in the specified location
}
function returnCurrentTimeAsDecimal()
{
    //returns the time as a decimal with the minutes being displayed as a percentage of an hour
    //e.g if the time was 12:30 this function would return 12.5. Used to decide what position the directional light will be in in game
    convertTimeZone();
    var hours = currentTimeInLocation.getHours();
    var mins = currentTimeInLocation.getMinutes() / 60; //converts the minutes to a percentage of an hour. percentage needs to be a decimal so does not need to be multiplied by 100
    return (hours + (Math.round(mins * 10) / 10))
}

function returnTimeAndDate()
{
    //returns the time and date as a full string to be displayed within the game
    const months = ["January", "February", "March", "April", "May", "June"
        , "July", "August", "September", "October", "November", "December"];
    const days = ["Sun", "Mon", "Tues", "Wed", "Thurs", "Fri", "Sat"];

    convertTimeZone();
    let h = currentTimeInLocation.getHours();
    let m = currentTimeInLocation.getMinutes();
    //let s = currentTimeInLocation.getSeconds();
    let day = days[currentTimeInLocation.getDay()];
    let date = currentTimeInLocation.getDate();
    let month = months[currentTimeInLocation.getMonth()];
    let year = currentTimeInLocation.getFullYear();
    if (m < 10) {
        m = "0" + m;
    }
    // if (s < 10) {
    //     s = "0" + s;
    // }
    return `${h} : ${m}  ${day} ${date} ${month} ${year}`
}

function returnTemperature()
{
    return `${Math.round(currentWeather?.main?.temp)}`;
}


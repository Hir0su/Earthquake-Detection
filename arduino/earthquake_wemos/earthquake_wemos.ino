// Wemos - WiFi 
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
// Wemos - WiFi - Time
#include <NTPClient.h>
#include <WiFiUdp.h>
#include <time.h>

const char* ssid = "SSID HERE";
const char* password = "PASSWORD HERE";
const char* host = "IP ADDRESS HERE";

WiFiUDP ntpUDP;
// 28800 to set timezone for PST
NTPClient timeClient(ntpUDP, "pool.ntp.org", 28800); 

int data_index = 0;
int intensity_level = 0;

String value_fetched1 = "";
String value_fetched2 = "";

void setup() {
  delay(2000);

  Serial.begin(9600);

  // Connecting to wifi print
  // Serial.print("Connecting to ");
  // Serial.println(ssid);
  // Start connecting
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  // loop for no connection
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    // Serial.println(".");
  }
  // Serial.println("WiFi connected");
  // Serial.println("IP address: ");
  // Serial.println(WiFi.localIP());
  pinMode(D8, OUTPUT);
  digitalWrite(D8, LOW);

  // Initialize NTP client
  timeClient.begin();   
}

void do_task(){
  // for this whole segment of lines
  // converts unix time (epoch) to date and time
  timeClient.update();
  time_t rawTime = timeClient.getEpochTime();
  struct tm *timeinfo = localtime(&rawTime);
  char date_val[20];
  char time_val[20];
  strftime(date_val, sizeof(date_val), "%Y-%m-%d", timeinfo);
  strftime(time_val, sizeof(time_val), "%H:%M:%S", timeinfo);
  //Serial.println(date_val);
  //Serial.println(time_val);

  // send data over WiFi
  if(WiFi.status()== WL_CONNECTED){
    HTTPClient http;
    WiFiClient wifi;
    String url = "http://IP ADDRESS HERE:8080/earthquake/collectMPU.php?ipsrc=1&dateVal=" + String(date_val) + "&timeVal=" + String(time_val) + "&intensityVal=" + String(intensity_level);
    http.begin(wifi, url);
    http.addHeader("Content-Type", "text/plain");
    http.GET();
    String res = http.getString();
    //Serial.println("Data sent!");
    //Serial.println(url);
    intensity_level = 0;
    delay(500);
  }
  else{
    Serial.println("Error in WiFi connection");
  }

  data_index = 0;
}

void send_db(){
String arr_data_received[3];

  while (Serial.available()) {
    if (data_index >= 3) {
      break; // Exit the loop if the array is full
    }

    String data_received = Serial.readStringUntil('\n');
    if (data_received != "\n" || data_received != ""){
      //Serial.println(data_received + "" + String(data_index)); serial monitor checker only
      arr_data_received[data_index] = data_received;
      data_index+=1;
    }
  }

  String data_check = arr_data_received[2];
    //Serial.println(data_check); serial monitor checker only
    String digits = "";

    for (int i = 0; i < data_check.length(); ++i) {
      if (isDigit(data_check[i])) {
        digits += data_check[i];
      }
    }

    if (digits == "1")
    {
      //Serial.println("Intensity Level 1");
      intensity_level = 1;
      do_task();
    } 
    else if (digits == "2"){
      //Serial.println("Intensity Level 2");
      intensity_level = 2;
      do_task();
    } 
    else if (digits == "3"){
      //Serial.println("Intensity Level 3");
      intensity_level = 3;
      do_task();
    }
}

void fetch_task_state(){
  if(WiFi.status()== WL_CONNECTED){
    HTTPClient http;
    WiFiClient wifi;
    String url = "http://IP ADDRESS HERE:8080/earthquake/StateData.php";
    http.begin(wifi, url);
    http.addHeader("Content-Type", "text/plain");
    http.GET();
    String fetched_data = http.getString();
    
    if (value_fetched1 != fetched_data){
      value_fetched1 = fetched_data;
      String val_for_send = value_fetched1;
      val_for_send.remove(0, 1);
      val_for_send.remove(val_for_send.length() - 1, 1);
      val_for_send.replace("\"", "");
      Serial.println(val_for_send);
    }
  }
  else{
    Serial.println("Error in WiFi connection");
  }
}

void fetch_task_duration(){
  if(WiFi.status()== WL_CONNECTED){
    HTTPClient http;
    WiFiClient wifi;
    String url = "http://IP ADDRESS HERE:8080/earthquake/DurationData.php";
    http.begin(wifi, url);
    http.addHeader("Content-Type", "text/plain");
    http.GET();
    String fetched_data = http.getString();
    
    if (value_fetched2 != fetched_data){
      value_fetched2 = fetched_data;
      String val_for_send = value_fetched2;
      val_for_send.remove(0, 1);
      val_for_send.remove(val_for_send.length() - 1, 1);
      val_for_send.replace("\"", "");
      Serial.println(val_for_send);
    }
  }
  else{
      Serial.println("Error in WiFi connection");
  }
}

void loop() {
  digitalWrite(D8, HIGH);
  delay(100);

  fetch_task_state();
  fetch_task_duration();
  send_db();

}
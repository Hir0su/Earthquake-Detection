// MPU6050 libraries/dependencies
#include <Wire.h>
#include <I2Cdev.h>
#include <MPU6050.h>

// Wemos - WiFi 
// #include <ESP8266WiFi.h>
// #include <ESP8266HTTPClient.h>

// Wemos - WiFi - Time
// #include <NTPClient.h>
// #include <WiFiUdp.h>
// #include <time.h>

MPU6050 mpu;
int buzzer = 2;

void setup() {
  Serial.begin(9600); //baud rate set up

   // Setup for buzzer
  pinMode(buzzer, OUTPUT);
  // Setup for I2C modules SDA-SCL
  Wire.begin(); // set SDA to pin A4, SCL to pin A5
  //Wire.setClock(400000); // use 400kHz I2C clock

  mpu.initialize(); // initialize MPU6050
  Serial.println("sensor on...");

  // Verify connection
  if (!mpu.testConnection()) {
    Serial.println("MPU6050 connection failed");
    while (1);
  }
}

void loop() {
  int16_t ax, ay, az, gx, gy, gz;
  mpu.getMotion6(&ax, &ay, &az, &gx, &gy, &gz); // get accelerometer and gyroscope data
  
  float accelMagnitude = sqrt(pow(ax, 2) + pow(ay, 2) + pow(az, 2)); // calculate the magnitude of the acceleration vector
  
  int intensity_level = 0;
  
  if (accelMagnitude > 17000 && accelMagnitude < 18000) { // if magnitude is greater than threshold, earthquake is detected
    intensity_level = 1;
    Serial.print("Acceleration magnitude: ");
    Serial.println(accelMagnitude);
    Serial.println("EARTHQUAKE DETECTED!");
    Serial.println("INTENSITY LEVEL: " + String(intensity_level));
    
    // buzzer segment 
    int buzz_counter = 0;
    while (buzz_counter < 5){
      buzz_counter += 1;
      tone(buzzer, 500); // turn on buzzer
      delay(1000); // delay for 1s
      noTone(buzzer); // resets the buzzer
      delay(500); // delay for .05s
    }
  } else if (accelMagnitude > 18000 && accelMagnitude < 21000) { //medium level
    intensity_level = 2;
    Serial.print("Acceleration magnitude: ");
    Serial.println(accelMagnitude);
    Serial.println("EARTHQUAKE DETECTED!");
    Serial.println("INTENSITY LEVEL: " + String(intensity_level));

    // buzzer segment
    int buzz_counter = 0; 
    while (buzz_counter < 5){
      buzz_counter += 1;
      tone(buzzer, 1000); // turn on buzzer
      delay(1000); // delay for 1s
      noTone(buzzer); // resets the buzzer
      delay(500); // delay for .05s
    }
  } else if (accelMagnitude > 21000) { //high level
    intensity_level = 3;
    Serial.print("Acceleration magnitude: ");
    Serial.println(accelMagnitude);
    Serial.println("EARTHQUAKE DETECTED!");
    Serial.println("INTENSITY LEVEL: " + String(intensity_level));

    // buzzer segment 
    int buzz_counter = 0;
    while (buzz_counter < 5){
      buzz_counter += 1;
      tone(buzzer, 1500); // turn on buzzer
      delay(1000); // delay for 1s
      noTone(buzzer); // resets the buzzer
      delay(500); // delay for .05s
    }
  }

  delay(50); // delay for 50 milliseconds
}

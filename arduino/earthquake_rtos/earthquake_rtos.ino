#include <Arduino_FreeRTOS.h>

// MPU6050 libraries/dependencies
#include <Wire.h>
#include <I2Cdev.h>
#include <MPU6050.h>

//#include <SoftwareSerial.h>

MPU6050 mpu;

// defining
int buzzer = 2;
int intensity_level = 0;
int buzz_counter = 0;
int buzzerFrequencies[3] = {500, 1500, 2000};
float accelMagnitude = 0;
int data_index = 0;

String duration_value = "";

// for RTOS
TaskHandle_t handler_receive;
TaskHandle_t handler_detect;
TaskHandle_t handler_buzz;
TaskHandle_t handler_send;

void setup() {
  Serial.begin(9600); //baud rate set up

   // Setup for buzzer
  pinMode(buzzer, OUTPUT);
  // Setup for I2C modules SDA-SCL
  Wire.begin(); // set SDA to pin A4, SCL to pin A5
  Wire.setClock(400000); // use 400kHz I2C clock

  mpu.initialize(); // initialize MPU6050
  //Serial.println("sensor on...");
  pinMode(8, OUTPUT);
  digitalWrite(8, LOW);

  // Verify connection
  if (!mpu.testConnection()) {
    Serial.println("MPU6050 connection failed");
    while (1);
  }

  // for RTOS
  xTaskCreate(Receive, "ReceiveTask", 100, NULL, 1, &handler_receive);
  xTaskCreate(Detect, "DetectTask", 100, NULL, 1, &handler_detect);
  xTaskCreate(Buzz, "BuzzTask", 100, NULL, 1, &handler_buzz);
  xTaskCreate(Send, "SendTask", 100, NULL, 1, &handler_send);
}

void loop()
{
}

static void Receive(void* pvParameters){
  while(1){

    String data_received;

    if (Serial.available()) {
      data_received = Serial.readStringUntil('\n');
      data_received.trim();

      int colonPos = data_received.indexOf(':');
      String key = data_received.substring(0, colonPos);
      String value = data_received.substring(colonPos + 1);
      key.trim();
      value.trim();

      if (key == "state") {

        if (value == "0")
        {
          vTaskSuspend(handler_detect);
          //Serial.println("turn off sensor");
        }
        else if (value == "1"){
          //Serial.println("turn on sensor");
          vTaskResume(handler_detect);
        }
      }

      if (key == "duration") {
        duration_value = value;
      }
    }
  }
}

static void Detect(void* pvParameters){
  while(1){
    digitalWrite(8, HIGH);
    vTaskSuspend(handler_buzz);
    vTaskSuspend(handler_send);

    intensity_level = 0;

    int16_t ax, ay, az, gx, gy, gz;
    mpu.getMotion6(&ax, &ay, &az, &gx, &gy, &gz); // get accelerometer and gyroscope data

    accelMagnitude = sqrt(pow(ax, 2) + pow(ay, 2) + pow(az, 2)); // calculate the magnitude of the acceleration vector

    if (accelMagnitude > 18000 && accelMagnitude < 19000) { // if magnitude is greater than threshold, earthquake is detected
      intensity_level = 1;

      vTaskResume(handler_buzz);
      vTaskResume(handler_send);
      vTaskDelay(100/portTICK_PERIOD_MS);
    }
    else if (accelMagnitude > 19000 && accelMagnitude < 22000) { //medium level
      intensity_level = 2;

      vTaskResume(handler_buzz);
      vTaskResume(handler_send);
      vTaskDelay(100/portTICK_PERIOD_MS);
    }
    else if (accelMagnitude > 22000) { //high level
      intensity_level = 3;

      vTaskResume(handler_buzz);
      vTaskResume(handler_send);
      vTaskDelay(100/portTICK_PERIOD_MS);
    }
  }
}

static void Send(void* pvParameters){
  while(1){
    vTaskSuspend(handler_detect);
    vTaskSuspend(handler_receive);

    Serial.println("Acceleration magnitude: " + String(accelMagnitude));
    Serial.println("EARTHQUAKE DETECTED!");

    if (intensity_level == 1) {
      Serial.println("INTENSITY LEVEL: " + String(intensity_level));
    }
    else if (intensity_level == 2) {
      Serial.println("INTENSITY LEVEL: " + String(intensity_level));
    }
    else if (intensity_level == 3) {
      Serial.println("INTENSITY LEVEL: " + String(intensity_level));
    }

    accelMagnitude = 0;

    delay(100);
    vTaskSuspend(handler_send);
    vTaskDelay(100/portTICK_PERIOD_MS);
  }
}

static void Buzz(void* pvParameters){
  while(1){
    vTaskSuspend(handler_detect);
    vTaskSuspend(handler_receive);

    int buzz_type = intensity_level - 1;

    if (duration_value  == 0){
      duration_value = 1;
    }

    while (buzz_counter < duration_value.toInt()){
      buzz_counter += 1;
      tone(buzzer, buzzerFrequencies[buzz_type]); // turn on buzzer
      delay(1000); // delay for 1s
      noTone(buzzer); // resets the buzzer
      delay(500); // delay for .05s
    }
    buzz_counter = 0;

    vTaskResume(handler_detect);
    vTaskResume(handler_receive);
    vTaskDelay(100/portTICK_PERIOD_MS);
  }
}

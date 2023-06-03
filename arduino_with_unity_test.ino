/// 本範例使用按鈕作為Arduino傳送
/// Led作為接收Unity訊息測試

void setup() {

//  Btn pin
  pinMode(8, INPUT);  
//  Led pin
  pinMode(10, OUTPUT);
  Serial.begin(9600);
}

// 隨便一個感測器測試用
int btnState;
int preState;

int preLedStatus;
int ledStatus;

void loop() {
  Serial.flush();
  btnState = digitalRead(8) == HIGH ? 1 : 0;
  if(btnState != preState){
    // 傳數值給Unity
    Serial.println(btnState);
    preState = btnState;
  }
  
  String input = Serial.readString();
  input.trim();
  if(input != ""){
    ledStatus = (input == "1") ? 1 : 0;
    if(ledStatus != preLedStatus){
      digitalWrite(10, ledStatus);
      preLedStatus = ledStatus;
    }
  }
  
  delay(10);
}

#include "Keyboard.h"

/*
   Layer Change Keys
   Change to layer 0 206
   Change to layer 1 207
   Change to layer 2 208
   Change to layer 3 209
   Change to layer 4 210
*/

byte changeLayer0 = 206;
byte changeLayer1 = 207;
byte changeLayer2 = 208;
byte changeLayer3 = 209;
byte changeLayer4 = 210;
byte layer = 1;  // max of 5 layers
int layers[5][6][8] =
{
  // Layer 1
  {
    {177,  194,  195,  196,  197,  198,  199,  0},
    {126,  49,   50,   51,   52,   53,   54,   0},
    {179,  113,  119,  101,  114,  116,  0,    0},
    {193,  97,   115,  100,  102,  103,  178,  0},
    {133,  122,  120,  99,   118,  98,   179,  0},
    {128,  131,  130,  0,    176,  32,   179,  0}
  },
  // Layer 2
  {
    {177,  194,  195,  196,  197,  198,  199,  0},
    {126,  49,   50,   51,   52,   53,   54,   0},
    {179,  113,  119,  101,  114,  116,  0,    0},
    {193,  97,   115,  100,  102,  103,  178,  0},
    {133,  122,  120,  99,   118,  98,   179,  0},
    {128,  131,  130,  0,    176,  32,   179,  0}
  },
  // Layer 3
  {
    {177,  194,  195,  196,  197,  198,  199,  0},
    {126,  49,   50,   51,   52,   53,   54,   0},
    {179,  113,  119,  101,  114,  116,  0,    0},
    {193,  97,   115,  100,  102,  103,  178,  0},
    {133,  122,  120,  99,   118,  98,   179,  0},
    {128,  131,  130,  0,    176,  32,   179,  0}
  },
  // Layer 4
  {
    {177,  194,  195,  196,  197,  198,  199,  0},
    {126,  49,   50,   51,   52,   53,   54,   0},
    {179,  113,  119,  101,  114,  116,  0,    0},
    {193,  97,   115,  100,  102,  103,  178,  0},
    {133,  122,  120,  99,   118,  98,   179,  0},
    {128,  131,  130,  0,    176,  32,   179,  0}
  },
  // Layer 5
  {
    {177,  194,  195,  196,  197,  198,  199,  0},
    {126,  49,   50,   51,   52,   53,   54,   0},
    {179,  113,  119,  101,  114,  116,  0,    0},
    {193,  97,   115,  100,  102,  103,  178,  0},
    {133,  122,  120,  99,   118,  98,   179,  0},
    {128,  131,  130,  0,    176,  32,   179,  0}
  }
};

int rows[6] = { A0, A1, A2, A3, A4, A5};

unsigned long waitTime[6][8] =
{
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()},
  {micros(), micros(), micros(), micros(), micros(), micros(), micros(), micros()}
};
unsigned long waitwait = 100000;  // Using Gateron Green found 100ms to be a enough of a debouncing time.  This could be reduced for linear switches.


void setup()
{
  // open the serial port:
  Serial.begin(9600);
  while (!Serial)
  { // wait for serial connection to be setup.
    ;
  }
  Serial.setTimeout(10);  // 10ms timeout instead of 1s

  // initialize control over the keyboard:
  Keyboard.begin();

  pinMode(A0, OUTPUT);
  pinMode(A1, OUTPUT);
  pinMode(A2, OUTPUT);
  pinMode(A3, OUTPUT);
  pinMode(A4, OUTPUT);
  pinMode(A5, OUTPUT);

  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);
  pinMode(4, INPUT_PULLUP);
  pinMode(5, INPUT_PULLUP);
  pinMode(6, INPUT_PULLUP);
  pinMode(7, INPUT_PULLUP);
  pinMode(8, INPUT_PULLUP);
  pinMode(9, INPUT_PULLUP);

  pinMode(13, OUTPUT);

  digitalWrite(13, HIGH);
  digitalWrite(A0, HIGH);
  digitalWrite(A1, HIGH);
  digitalWrite(A2, HIGH);
  digitalWrite(A3, HIGH);
  digitalWrite(A4, HIGH);
  digitalWrite(A5, HIGH);
}
//
void loop()
{
  /*
     Every 70 minuites of keyboard function there will be a short reset
     This reset is set to be 100ms long
     Reducing this number could cause issues of keyboard not being functinoal for 70 minuites.
  */
  if (micros() < 100000)
  {
    for (byte i = 0; i < 6; i++)
    {
      for (byte j = 0; j < 8; j++)
      {
        waitTime[i][j] = micros();
      }
    }
    Serial.write("Reset time");
  }

  // This goes through of the rows, and scans it across each of the colums.
  for (byte i = 5; i >= 0; i--)
  {
    digitalWrite(rows[i], LOW);
    for (byte j = 2; j < 10; j++)
    {
      if (digitalRead(j) == 0)
      {
        if ((micros() - waitTime[i][j]) > waitwait)
        {
          Keyboard.press(layers[0][i][j - 2]);
          waitTime[i][j] = micros();
          Serial.println("A button was pressed");
        }
      }
      else
      {
        Keyboard.release(layers[0][i][j - 2]);
      }
    }
    digitalWrite(rows[i], HIGH);
  }
}
//
//    // Row 1.
//    digitalWrite(A5, LOW);
//    byte i = 0;
//    for (byte j = 2; j < 10; j++)
//    {
//      if (digitalRead(j) == 0)
//      {
//        if ((micros() - waitTime[i][j]) > waitwait)
//        {
//          Keyboard.write(layers[layer][i][j - 2]);
//          waitTime[i][j] = micros();
//        }
//      }
//    }
//    digitalWrite(A5, HIGH);
//
//    // Row2
//    digitalWrite(A4, LOW);
//    i = 1;
//    for (byte j = 2; j < 10; j++)
//    {
//      if (digitalRead(j) == 0)
//      {
//        if ((micros() - waitTime[i][j]) > waitwait)
//        {
//          Keyboard.press(layers[0][i][j - 2]);
//          waitTime[i][j] = micros();
//        }
//      }
//      else
//      {
//        Keyboard.release(layers[0][i][j - 2]);
//      }
//    }
//    digitalWrite(A4, HIGH);
//
//
//    // Row3
//    digitalWrite(A3, LOW);
//    i = 2;
//    for (byte j = 2; j < 10; j++)
//    {
//      if (digitalRead(j) == 0)
//      {
//        if ((micros() - waitTime[i][j]) > waitwait)
//        {
//          Keyboard.press(layers[0][i][j - 2]);
//          digitalWrite(13, HIGH);
//          waitTime[i][j] = micros();
//          Serial.println("A button was pressed");
//        }
//      }
//      else
//      {
//        Keyboard.release(layers[0][i][j - 2]);
//      }
//    }
//    digitalWrite(A3, HIGH);
//
//
//    // Row4
//    digitalWrite(A2, LOW);
//    i = 3;
//    for (byte j = 2; j < 10; j++)
//    {
//      if (digitalRead(j) == 0)
//      {
//        if ((micros() - waitTime[i][j]) > waitwait)
//        {
//          Keyboard.press(layers[0][i][j - 2]);
//          waitTime[i][j] = micros();
//        }
//      }
//      else
//      {
//        Keyboard.release(layers[0][i][j - 2]);
//      }
//    }
//    digitalWrite(A2, HIGH);
//
//
//    // Row5
//    digitalWrite(A1, LOW);
//    i = 4;
//    for (byte j = 2; j < 10; j++)
//    {
//      if (digitalRead(j) == 0)
//      {
//        if ((micros() - waitTime[i][j]) > waitwait)
//        {
//          Keyboard.press(layers[0][i][j - 2]);
//          waitTime[i][j] = micros();
//        }
//      }
//      else
//      {
//        Keyboard.release(layers[0][i][j - 2]);
//      }
//    }
//    digitalWrite(A1, HIGH);
//
//    // Row6
//    digitalWrite(A0, LOW);
//    i = 5;
//    for (byte j = 2; j < 10; j++)
//    {
//      if (digitalRead(j) == 0)
//      {
//        if ((micros() - waitTime[i][j]) > waitwait)
//        {
//          Keyboard.press(layers[0][i][j - 2]);
//          waitTime[i][j] = micros();
//        }
//      }
//      else
//      {
//        Keyboard.release(layers[0][i][j - 2]);
//      }
//    }
//    digitalWrite(A0, HIGH);
//}

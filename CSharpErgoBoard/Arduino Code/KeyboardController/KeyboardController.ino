#include "Keyboard.h"

byte layer = 0;  // max of 5 layers
int layers[5][6][8] = // 5 layers, 6 rows, 8 columns.
{
  // Layer 0
  {
    {177,  194,  195,  196,  197,  198,  199, 0},
    {96,   49,   50,   51,   52,   53,   1,   0},
    {179,  113,  119,  101,  114,  116,  2,   0},
    {193,  97,   115,  100,  102,  103,  3,   0},
    {133,  122,  120,  99,   118,  98,   4,   0},
    {128,  131,  130,  0,    176,  32,   179, 0}
  },
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
  }
};

int rows[6] = {A5, A4, A3, A2, A1, A0}; // The keys for the rows. This exists because the A series is hard to convert to numbers.

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
  //Serial
  Serial.begin(9600);
  Serial.setTimeout(100);  // 10ms timeout instead of 1s
  //Keyboard
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

void loop()
{
  /*
     Every 70 minuites of keyboard function there will be a short reset
     This reset is set to be 100ms long
     Reducing this number could cause issues of keyboard not being functinoal for 70 minuites.
  */
  // Time reset.
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

  //Serial Control
  if (Serial.available())
  {
    String command = Serial.readString();
    if (command.substring(0, 4) == "Name")
    {
      Serial.println("Left Keyboard");
    }
    else if (command.startsWith("Get"))
    {
      int commandLayer = command.substring(4, 6).toInt();
      int commandRow = command.substring(6, 7).toInt();
      int commandCol = command.substring(8, 9).toInt();
      Serial.println(layers[commandLayer][commandRow][commandCol]);
    }
    else if (command.startsWith("Set"))
    {
      int commandLayer = command.substring(4, 6).toInt();
      int commandRow = command.substring(6, 7).toInt();
      int commandCol = command.substring(8, 9).toInt();
      int commandValue = command.substring(10).toInt();
      layers[commandLayer][commandRow][commandCol] = commandValue;
    }
  }

  // Keyboard Control
  for (byte i = 0; i <= 6; i++)
  {
    digitalWrite(rows[i], LOW);
    for (byte j = 2; j < 10; j++)
    {
      if (digitalRead(j) == 0)
      {
        if ((micros() - waitTime[i][j]) > waitwait)
        {
          // If layer change.
          if (layers[layer][i][j - 2] < 6) // Special keys.
          {
            int key = layers[layer][i][j - 2];
            if (key == 0)
            {
              continue; // This is the null key.
            }
            else if (key == 1) 
            {
              layer = 0;
            }
            else if (key == 2) 
            {
              layer = 1;
            }
            else if (key == 3)
            {
              layer = 2;
            }
            else if (key == 4)
            {
              layer = 3;
            }
            else if (key == 5)
            {
              layer = 4;
            }
          }
          else  // Character/command keys.
          {
            Keyboard.press(layers[layer][i][j - 2]);
            waitTime[i][j] = micros();
          }
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

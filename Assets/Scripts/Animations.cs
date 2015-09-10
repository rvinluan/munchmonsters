using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class Animations {

  public static float CubicEaseInOut( float t, float b, float c, float d ){
      if ( ( t /= d / 2 ) < 1 )
          return c / 2 * t * t * t + b;

      return c / 2 * ( ( t -= 2 ) * t * t + 2 ) + b;
  }

  public static float QuintEaseIn(float t, float b, float c, float d) {
    return c*(t/=d)*t*t*t*t + b;
  }

  public static float QuintEaseOut(float t, float b, float c, float d) {
    return c*((t=t/d-1)*t*t*t*t + 1) + b;
  }

  public static float QuintEaseInOut(float t, float b, float c, float d) {
    if ((t/=d/2) < 1) return c/2*t*t*t*t*t + b;
    return c/2*((t-=2)*t*t*t*t + 2) + b;
  }
}

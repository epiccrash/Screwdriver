using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class GetDrunkScript : MonoBehaviour
{

    private enum DrunkState
    {
        LevelZero,
        LevelOne,
        LevelTwo
    }

    private float _alcoholByVolume;
    private BlurOptimized _blur;
    private Vortex _vortex;
    private DrunkState _state;
    private const float EPSILON = 0.01f;

    private float _drunkStartTime;

    // Start is called before the first frame update
    void Start()
    {
        _blur = GetComponent<BlurOptimized>();
        _vortex = GetComponent<Vortex>();
        _state = DrunkState.LevelZero;
        ResetBlur();
        ResetVortex();
        // StartCoroutine(SoberUp());
        // Every 20 seconds you can start to sober
        // Ping pong the vortex angle
        // Slowly ramp up the drunk blur
    }

    // Update is called once per frame
    void Update()
    {
        print("Alc: " + _alcoholByVolume);
        // arbitrary funciton for now, will replace with more sensible function soon
        //_vortex.angle = (_alcoholByVolume * 20) - Mathf.PingPong(Time.time, _alcoholByVolume * 10); 
        //if (_alcoholByVolume > 0.01f)
        //{
        //float vortexMin = _alcoholByVolume * -10;
        //float vortexMax = _alcoholByVolume * 10;
        float vortexMin = -10.0f;
        float vortexMax = 10.0f;

            // float subtractAmt = Mathf.Min(_alcoholByVolume * 60, 80);
            // _vortex.angle = (Mathf.PingPong(Time.time * 60, 100) - subtractAmt);
        _vortex.angle = (Mathf.PingPong(Time.time, vortexMax - vortexMin) + vortexMin);
        print("Vortex Angle:" + _vortex.angle);
        //}        

        if (Time.time - _drunkStartTime > 5 && _alcoholByVolume > 0.1f)
        {
            UpdateAlc(-Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Water"))
        {
            IngredientType ingredient = other.gameObject.GetComponent<IngredientScript>().IngredientType;
            int enumVal = (int)ingredient; // Calculating alcohol content of the drink
            if (enumVal < 100)
            {
                UpdateAlc((int)ingredient / 100.0f); // Value of the enum is the % alc of the drink
                
                _drunkStartTime = Time.time;
            }
        }
    }

    public float GetABV()
    {
        return _alcoholByVolume;
    }

    private void ResetBlur()
    {
        _blur.enabled = true;
        _blur.downsample = 0;
        _blur.blurSize = 0;
        _blur.blurIterations = 0;
        
    }

    private void ResetVortex()
    {
        _vortex.enabled = true;
        _vortex.radius.x = 0.5f;
        _vortex.radius.y = 0.5f;
        _vortex.center.x = 0.5f;
        _vortex.center.y = 0.5f;
        _vortex.angle = 0;
    }

    private void UpdateAlc(float incAlc)
    {
        if (_alcoholByVolume >= 0)
        {
            _alcoholByVolume += incAlc;
        }

        UpdateBlur(incAlc);
    }

    private void UpdateBlur(float incBlur)
    {
        //if (System.Math.Abs(_alcoholByVolume) < EPSILON && _state == DrunkState.LevelZero) // if alc level == 0
        //{
        //    BlurLevelOne();
        //}
        //else if (System.Math.Abs(_blur.blurSize - 2.7f) < EPSILON && _state == DrunkState.LevelOne) // blur size == 2.7
        //{
        //    BlurLevelTwo();
        //}
        //else
        //{
        //    _blur.blurSize += incBlur;
        //}

        // Simple blur function to determine if everything is set up correctly
        if ((_blur.blurSize < 4.5f && incBlur > 0) || (_blur.blurSize >= 0 && incBlur <= 0)) {
            _blur.blurSize += incBlur;
        }

        _blur.blurIterations = (int) Mathf.Max(1, Mathf.Floor(_blur.blurSize));
    }

    private void BlurLevelOne()
    {
        _blur.enabled = true;
    }

    private void BlurLevelTwo()
    {
        _blur.blurSize = 1.2f;
        _blur.blurIterations = 2;
    }

    private IEnumerator SoberUp()
    {
        while (true)
        {
            if (_alcoholByVolume > 0.1f)
            {
                UpdateAlc(-0.1f);
                yield return new WaitForSeconds(5.0f);
            }
        }
    }

}

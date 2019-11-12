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

    public GameObject VRCamera;

    private float _alcoholByVolume;
    private BlurOptimized _blur;
    private Vortex _vortex;
    private DrunkState _state;
    private const float EPSILON = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        _blur = VRCamera.GetComponent<BlurOptimized>();
        _vortex = VRCamera.GetComponent<Vortex>();
        _state = DrunkState.LevelZero;
        ResetBlur();
        ResetVortex();
        StartCoroutine(SoberUp());
        // Every 20 seconds you can start to sober
        // Ping pong the vortex angle
        // Slowly ramp up the drunk blur
    }

    // Update is called once per frame
    void Update()
    {
        // arbitrary funciton for now, will replace with more sensible function soon
        _vortex.angle = (_alcoholByVolume * 10) - Mathf.PingPong(Time.time, _alcoholByVolume * 20); 

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Water"))
        {
            IngredientType ingredient = collision.gameObject.GetComponent<IngredientScript>().IngredientType;
            int enumVal = (int)ingredient; // Calculating alcohol content of the drink
            if (enumVal < 100)
            {
                UpdateAlc((int)ingredient / 100.0f); // Value of the enum is the % alc of the drink
                
            }
        }
    }

    public float GetABV()
    {
        return _alcoholByVolume;
    }

    private void ResetBlur()
    {
        _blur.downsample = 0;
        _blur.blurSize = 0;
        _blur.blurIterations = 1;
        _blur.enabled = false;
    }

    private void ResetVortex()
    {
        _vortex.radius.x = 0.5f;
        _vortex.radius.y = 0.5f;
        _vortex.center.x = 0.5f;
        _vortex.center.y = 0.5f;
        _vortex.angle = 0;
    }

    private void UpdateAlc(float incAlc)
    {
        _alcoholByVolume += incAlc;
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
        _blur.blurSize += incBlur;
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

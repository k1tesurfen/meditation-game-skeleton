using UnityEngine;

public class PostProcess : MonoBehaviour
{
    private Material _material;
    public Shader _shader;
    public float energyValue;

    // Start is called before the first frame update
    void Start()
    {
       _material = new Material(_shader); 
    }
    
    void Update(){
        _material.SetFloat("_Energy", energyValue);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination){
        Graphics.Blit(source, destination, _material);
    }
}

using UnityEngine;
using Pcx;

[ExecuteInEditMode]
public class PointDeform : MonoBehaviour {

    // Executed in the CPU
    [SerializeField] PointCloudData _sourceData; // Get the data from file
    [SerializeField] ComputeShader _computeShader; // compute shader 

    // Variables
    [SerializeField] float _twist;
    [SerializeField] float _stretch;
    [SerializeField] float _bend;

    // Final calcculation
    ComputeBuffer _pointBuffer;

    void OnDisable()
    {
        if (_pointBuffer != null)
        {
            _pointBuffer.Release();
            _pointBuffer = null;
        }
    }

    void Update()
    {
        if (_sourceData == null) return;

        // nuew varia^ble souce data is the point cloud
        var sourceBuffer = _sourceData.computeBuffer;

        // if there is no data dont do anything 
        if (_pointBuffer == null || _pointBuffer.count != sourceBuffer.count)
        {

            // if there is something we renitailise
            if (_pointBuffer != null) _pointBuffer.Release();

            // we fills the pointBuffer with our actual data
            _pointBuffer = new ComputeBuffer(sourceBuffer.count, PointCloudData.elementSize);
        }

        // if aplication is running get time othrewise 0
        var time = Application.isPlaying ? Time.time : 0;

        // adress the correct program (kernel)
        var kernel = _computeShader.FindKernel("Main");

        // Set value to the shader
        _computeShader.SetFloat("Twist", _twist);
        _computeShader.SetFloat("Stretch", _stretch);
        _computeShader.SetFloat("Bend", _bend);
        _computeShader.SetFloat("Time", time);

        // SourceBuffer original
        _computeShader.SetBuffer(kernel, "SourceBuffer", sourceBuffer);

        // Resul 
        _computeShader.SetBuffer(kernel, "OutputBuffer", _pointBuffer);

        // This does the calculation, execution
        _computeShader.Dispatch(kernel, sourceBuffer.count / 128, 1, 1);

        // Send data to another script
        GetComponent<PointCloudRenderer>().sourceBuffer = _pointBuffer;
    }
}

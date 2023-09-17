Shader "Outlines/BackFaceOutlines" {
    Properties {
        _Thickness ("Thickness", Float) = 1 // The amount to extrude the outline mesh
        _Color ("Color", Color) = (1, 1, 1, 1) // The outline color
        // If enabled, this shader will use "smoothed" normals stored in TEXCOORD1 to extrude along
        [Toggle(USE_PRECALCULATED_OUTLINE_NORMALS)]_PrecalculateNormals("Use UV1 normals", Float) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass {
            Name "Outlines"
            // Cull front faces
            Cull Front

            HLSLPROGRAM
            // Standard URP requirements
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            // Register our material keywords
            #pragma shader_feature USE_PRECALCULATED_OUTLINE_NORMALS

            // Register our functions
            #pragma vertex Vertex
            #pragma fragment Fragment

            #ifndef BACKFACEOUTLINES_INCLUDED
            #define BACKFACEOUTLINES_INCLUDED

            // Include helper functions from URP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Data from the meshes
            struct Attributes {
                float4 positionOS       : POSITION; // Position in object space
                float3 normalOS         : NORMAL; // Normal vector in object space
            #ifdef USE_PRECALCULATED_OUTLINE_NORMALS
                float3 smoothNormalOS   : TEXCOORD1; // Calculated "smooth" normals to extrude along in object space
            #endif
            };

            // Output from the vertex function and input to the fragment function
            struct VertexOutput {
                float4 positionCS   : SV_POSITION; // Position in clip space
            };

            // Properties
            float _Thickness;
            float4 _Color;

            VertexOutput Vertex(Attributes input) {
                VertexOutput output = (VertexOutput)0;

                float3 normalOS = input.normalOS;
            #ifdef USE_PRECALCULATED_OUTLINE_NORMALS
                normalOS = input.smoothNormalOS;
            #else
                normalOS = input.normalOS;
            #endif

                // Extrude the object space position along a normal vector
                float3 posOS = input.positionOS.xyz + normalOS * _Thickness;
                // Convert this position to world and clip space
                output.positionCS = GetVertexPositionInputs(posOS).positionCS;

                return output;
            }

            float4 Fragment(VertexOutput input) : SV_Target{
                return _Color;
            }

            #endif 


            ENDHLSL
        }
    }
}
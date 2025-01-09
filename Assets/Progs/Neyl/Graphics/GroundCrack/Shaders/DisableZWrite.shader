Shader"Neyl/DisableZwrite" { 
    Properties {

        
    }
    SubShader {
Tags{
    "RenderType" = "Opaque"
}
        Pass {
Zwrite Off
    }
}
}

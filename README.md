# Retro Wave Landscape Generator Design Document (Unity URP Version)

## 1. Introduction
The Retro Wave Landscape Generator for Unity URP (Universal Render Pipeline) will encapsulate the quintessential aesthetics of the 1980s retro-futuristic visuals, celebrated in synthwave/retrowave art. By leveraging Unity's URP, the tool will allow users to generate stylized landscapes imbued with vibrant neon colors, glowing horizons, and abstract mountain silhouettes. It will tap into the powerful rendering capabilities of URP to achieve high-performance visuals suitable for both game development and standalone digital art.

## 2. Goal

The project's primary objective is to build an interactive, procedural generation system within Unity URP that crafts landscapes resonating with retro wave artistry. The tool will empower users to modify aspects such as color schemes, terrain contours, and atmospheric effects to forge unique scenes reminiscent of an 80s digital dreamscape.

### The tool's design goals include:
- Creation of visually compelling landscapes with real-time modifications.
- User-friendly interface for manipulating generation parameters.
- Pre-configured themes that represent various retro wave styles.
- Optimization for efficient performance in Unity's URP environment.
- Capability to export high-quality renders and potentially support in-game deployment.

## 3. Specification
- Unity Version: Compatibility with the latest stable release of Unity at the time of development.
- Rendering Pipeline: Tailored specifically for Unity's Universal Render Pipeline.
- Content Generation (the following are procedurally generated, specific meshes such as cars, palm trees, road signs and lamps have to be imported by the user.

### Terrain
- The sun/ the moon that appears in the majority of synthwave art
### Skybox texture
- Customizability:
- Tessellation, “gridification” of user imported mesh and possibly other stylization processing.
- Terrain generation parameters

### Bloom effect
- Post-processing VFX such as stars, outlines, etc.
- Tone mapping and Color Scheme (to switch between sunrise and sunset variants)
- Particle Effects used for smoke, fog, rain, etc.
- Optional foreground text with customizable font
  - URP Post-processing: Use of URP's post-processing stack for bloom, color grading, and other visual enhancements.
  - Performance: Use of LODs, culling, and other optimization techniques to ensure the system runs smoothly.

## 4. Techniques
- Perlin Noise and Simplex Noise: To procedurally generate the landscape and control its retro stylization.
- Shader Graph: For creating custom shaders.
- Custom Scriptable Render Features: To add specific rendering passes and script the pipeline.
- Scriptable Objects: For storing and manipulating generation parameters and presets.
- Post-processing Stack: To enhance visual fidelity with bloom, chromatic aberration, and color adjustments, etc.
- (Stretch Goal) Procedural Skybox: To simulate the changing sky conditions and lighting consistent with the retro wave theme.

## 5. Timeline
- Week 1: Initial Setup and Basic Functionality
  - Project setup in Unity with URP, research, and asset gathering.
  - Basic terrain generation with adjustable parameters.
  - Basic stylized sun/moon generation with adjustable parameters.
  - Basic UI for tweaking and debugging.
  - Implementation of a simple terrain gridifying system using Shader Graph.
- Week 2: Advanced Features and Aesthetics
  - Crafting of URP post-processing effects for the authentic retro wave look.
  - UI improvements.
  - Implementation of post-processing passes using Shader Graph.
  - Particle Effects.
- Week 3: Polishing, Optimization, and Even More Advanced Features
  - Polishing UI, adding presets, and further refinement of shaders.
  - Comprehensive testing and performance optimization.
  - Final touches and bug fixes based on test feedback.
  - Project documentation and example gallery preparation.
  - Project wrap-up and submission.
  - (If time permits) Development of skybox with time-of-day changes and custom shaders.
  - (If time permits) Creation of export functionalities for artwork or game assets.
 
## 6. Work division
- Shixuan Fang: procedural skybox and terrain
- Tongwei Dai: Particle system(like stars, birds, etc.), Interactive scene
- Gehan Zheng: Terrain shaders, post-process shaders, tone mapping, and other potential shaders

## Milestone1
### Works Done:
- **Tongwei Dai**
  - Add gridification shader for the ground plane
  - Car exhaust particle effect
- **Gehan Zheng**
  - Add screen space post process render feature for tone mapping
  - Add toon-based post process materials for car
- **Shixuan Fang**
  - Finished URP setup
  - Added custom post process features and passes
  - Sun shader in progress
### Current Result
- <img width="1280" alt="5ae1f9451e39f5789adea058fa98cb3" src="https://github.com/dw218192/UnityURP_Synthwave_Sytalization/assets/31294154/fa5083c5-099b-481a-9d71-e6421a1b552b">



## Milestone #2
### Works Done:

- **Tongwei Dai**
  - Create a procedural terrain that can mimic infinite road
  - Add physics to the car so that it can move up and down while moving forward

- **Gehan Zheng**
  - Add volumetric fog to the car and the scene to increase the visual quality of the scene
  - Build the sample scene by adding moving palm trees and moving screen space reflection road

- **Shixuan Fang**
  - Finished the procedural sun with controllable parameters
  - Add a color gradient sky to mimic the desirable visual result
### Result

https://github.com/dw218192/UnityURP_Synthwave_Sytalization/assets/31180310/0a5d22d2-77f0-4c90-aa5c-6bcb6888e20f


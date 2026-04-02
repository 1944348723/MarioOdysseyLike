[TOC]

# 相机
## Cinemachine
### 简介
`Cinemachine`的核心就是`CinemachineVirtualCamera`和`CinemachineBrain`这两个组件。

`CinemachineVirtualCamera`(后面用`vcam`简称)中是一些相机镜头参数配置、移动方式、朝向方式以及priority，它本身并不是相机(不渲染)，而是控制`Camera`的，对应了虚拟相机这个名字。当它处于`live`状态时，就是在控制相机。可以同时有多个`vcam`，但除了`blend`(从一个`vcam`切换到另一个`vcam`)时，只会有一个`vcam`处于`live`状态。

`CinemachineBrain`主要是监控`CinemachineVirtualCamera`，根据`vcam`组件是否激活以及它们的`priority`大小来决定使用哪个`vcam`，会使用激活状态下的`priority`大的`vcam`，将`vcam`的计算结果应用到`Camera`上

### CinemachineVirtualCamera的Follow和Body以及Body的BindingMode
`Follow`和`Body`共同决定了相机如何移动

`Follow`属性是一个`Transform`，表示跟随谁

`Body`表示具体移动策略，有七个选项

* **Do Nothing**：不做任何位置计算，适合全手写控制时用。
* **Hard Lock To Target**：相机位置硬锁到`Follow`目标点，
* **Transposer**：按一个固定偏移(offset)跟随目标，可加阻尼，`offset`的参考坐标系通过`Binding Mode`确定；最通用、最基础的第三人称/跟随方案。
* **Framing Transposer**：重点是“屏幕构图跟随”，让目标稳定处在屏幕某区域（deadzone、softzone、no pass area）；2D、横版、2.5D 特别常用。
* **Orbital Transposer**：在Transposer基础上，可以绕Up轴旋转(通过鼠标x轴、右摇杆输入)，并且可以自动回正；经典第三人称“环绕角色”机位。
* **Third Person Follow**：专为肩后第三人称设计（肩偏移、臂长、相机距离、阻尼、避障），自带避障，直接配置就行，不用额外添加`CinemachineCollider`。；TPS 手感通常比纯 Transposer 更自然。
* **Tracked Dolly**：相机沿预设路径（Dolly Track）移动，并可跟随路径进度；过场、隧道、固定轨道镜头常用

`BindingMode`定义了 Unity 用于解释相机相对于目标的偏移量和阻尼的坐标空间。在选择`Transposer`和`Orbital Transposer`时可以设置。`BindingMode`有以下六种

* **Lock To Target**：使用`Follow`目标的本地坐标，`Follow`目标旋转时，相机位置也会跟着旋转，三个轴都是
* **Lock To Target No Roll**：使用`Follow`目标的本地坐标，但`Follow`目标Roll(沿z轴旋转)时，相机位置不会变化
* **Lock To Target On Assign**：在相机激活或者设置`Follow Target`时，使用`Follow Target`的本地坐标，后续`offset`在世界坐标中保持不变。简单来说就是使用`Follow Target`的本地坐标，但完全不会随`Follow Target`的旋转改变位置。
* **Lock To Target With World Up**：使用`Follow Target`的本地坐标，只跟随Yaw(沿y轴旋转)发生位置变化
* **World Space**：使用世界坐标
* **Simple Follow With World up**：相机“尽量少移动”去保持与目标的距离和高度，类似人在跟拍。很模糊，没太懂

### CinemachineVirtualCamera的LookAt和Aim
`LookAt`和`Aim`共同决定了相机如何旋转

`LookAt`属性是一个`Transform`，表示看向谁

`Aim`表示旋转策略，有六个选项

* **Do Nothing**：不做控制
* **Hard Look At**：每帧直接看向`Look At Target`，目标始终在画面中心
* **Composer**：保持`Look At Target`在画面中，使用“屏幕构图规则”，即deadzone、softzone等配置
* **Group Composer**：Composer的多目标版
* **POV**：用输入驱动相机yaw(绕y轴旋转)/pitch(绕x轴旋转)
* **Same As Follow Target**：相机旋转直接和`Look At Target`旋转一致

## 本项目的相机
本项目使用纯手写相机脚本的方式，核心就是在距离目标点distance的球面上环绕，包括通过输入设备手动环绕和根据速度自动环绕，并且对y轴进行死区判定，避免频繁抖动。
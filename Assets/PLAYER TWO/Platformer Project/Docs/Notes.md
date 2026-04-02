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

### 画面抖动bug
**1.问题**

通过鼠标移动来让相机手动环绕角色时感觉视角在抖动

**2.背景**

我最先实现的就是手动控制环绕，并且当时测试没有问题。接着实现了相机根据角色速度自动环绕，测试也没问题，但是这里我并没有再测试手动环绕有没有问题。接着准备实现相机碰撞时发现手动环绕时有问题。

**3.解决过程**

由于我之前已经验证过手动环绕没有问题，并且在此之后只提交过一次实现自动环绕的代码，肯定是这次提交的代码引入的问题，所以定位起来应该是不会很难的。

出现问题后我的第一想法就是手动环绕和自动环绕可能出现了一些冲突，毕竟两个是同时生效的，而且我在实现自动环绕时其实就想过两个会不会冲突，当时想着可以在手动环绕时暂时取消自动环绕，或者抑制自动环绕，让它的作用变小。但是其实手动和自动造成的影响是叠加的，那么按理来说应该是不会抖动，顶多就是手动环绕时由于叠加了自动环绕，会感觉环绕的比预想的快或者比预想的要慢。并且想着先写简单点，后面要是真的有问题再解决。于是我就先把自动环绕关了，再去测试下，看看是不是冲突的问题，结果发现并不是，关了自动环绕之后手动环绕依然有问题。

那就不是自动环绕的问题了，我再检查了下这次提交还改了什么代码，发现加了一行代码将跟随目标挂在了相机节点下，将这一行注释掉之后再次测试，问题就没有了。那么问题就是出在这里了，简单想一下能感觉到这样确实会出问题，target是camera的子节点，那当camera移动的时候target也会跟着移动，而target和camera本身就是有一定的空间依赖关系的。但是真正去看代码捋清楚到底为什么会抖动还是挺复杂的

**4.问题是如何发生的**

我也只能大致分析一下，感觉这个过程太复杂了，有点混沌的感觉

首先，理想情况下，我们的相机是一直在以target为球心的一个球面上环绕，并且始终看向target的，环绕时镜头是很丝滑的，因为这个过程是连续的。

大致的流程是根据输入和角色速度算出camera相对target的pitch和yaw，然后同步target和player的位置，再根据target的位置和pitch、yaw算出camera的位置，再让camera看向target。

* 由于我们是通过yaw、pitch算出camere是在target固定距离的球面上哪一点，而target在每帧开始时都会同步player的位置，所以在计算camera的位置时，用的始终都是player的位置`T`，target此时还没有收到影响。所以相机的`C0->C1->C2`...是始终在player固定距离的球面上连续移动
* 每次camera移动就会导致target跟着移动`T1->T2->T3`...，并且每次移动都是在`T`的基础上，而不是在`Tn-1`的基础上。camera距离player是有一段距离的，所以yaw、pitch的变化导致Camera每次位移的变化会比较大，而target是跟随Camera移动的，再叠加上camera的旋转也会导致target跟着有较大的位移。也就是每帧都是`T->T1`、`T->T2`这样的位移，并且位移还比较大。
* 而每帧结束时，camera都是`C1->T1`、`C2->T2`这样的视线，很难说这样的变化会是规律连续的，
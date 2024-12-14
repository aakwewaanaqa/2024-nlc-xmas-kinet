## 前置作業
1. KINECT SDK：<BR>
   https://www.microsoft.com/en-us/download/details.aspx?id=44561
2. Unity 6000.0.26f1<BR>
   https://unity.com/cn/releases/editor/archive
3. Windows 電腦

## 遊戲流程
1. 顯示選單畫面，畫面中禮物掉落塞滿，夾娃娃機
2. 透過`Kinect`的方式將手壓在標題上開始遊戲
3. 遊戲開始，透過`Kinect`的方式將手壓在`<-`或`->`上移動夾子
4. 移動到目標位置後透過`Kinect`的方式將手壓在`抓`上抓取禮物
5. 禮物抓到之後挑出畫面，顯示`上帝的話`(像是天父小卡一樣)
6. 等待玩家決定跳過(透過`Kinect`的方式將手壓在`->`跳過)或是時間到之後(`30秒`)回到`1.`

## 細節
主要場景是`Assets/GameUsed/Scenes/Title/title.unity`<BR>
場景由`Main.cs`執行<BR>
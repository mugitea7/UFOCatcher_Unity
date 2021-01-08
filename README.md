# UFOCatcherProgram_Unity
UnityのUFOキャッチャーのプログラム

# ArmController
アームを移動

# AudioController
GameManagerのStateによりSE再生

# DropScript
景品排出口にisTriggerなオブジェクトにアタッチして使用.
オブジェクトが接触したら景品ゲット.

# GameManager
ステート管理, カメラ位置, 景品管理, 

# LightsController
ボタンの点灯を制御

# PrizesController
景品を管理するスクリプト.
StartでListに設定した景品分GameManagerの景品にAdd
景品を再配置するボタンも管理

# UFOController
UFOの移動管理スクリプト.
ややこしいです.
アームで景品キャッチ後, そのまま動かすと景品に物理演算が働かないのでアームの親子を切って移動させています.
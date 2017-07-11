import os;
import sys;

enemyBehaviorFile = 'CodeGenerator/EnemyBehaviorX.cs'
behaviorAssetFile = 'CodeGenerator/EnemyBehaviorXAsset.cs'
assetEditorFile = 'CodeGenerator/EnemyBehaviorXAssetEditor.cs'
assetForJsonFile = 'CodeGenerator/EnemyBehaviorXAssetForJson.cs'
enemyBehaviorGenPath = '../Assets/Scripts/Game/Character/EnemyBehavior/EnemyBehavior$name$.cs'
behaviorAssetGenPath = '../Assets/Scripts/Game/Character/EnemyBehaviorAsset/EnemyBehavior$name$Asset.cs'
assetEditorGenPath = '../Assets/Scripts/Editor/EnemyBehavior$name$AssetEditor.cs'
assetForJsonPath = '../Assets/Scripts/Game/Character/EnemyBehaviorAssetForJson/EnemyBehavior$name$AssetForJson.cs'

def copy(source, destination, name):
    with open(source, 'r') as f:
        str = f.read()
    str = str.replace('$name$', name)
    genPath = destination.replace('$name$', name)
    with open(genPath, 'w') as f:
        f.write(str)

if __name__ == '__main__':
    name = sys.argv[1]
    copy(enemyBehaviorFile, enemyBehaviorGenPath, name)
    copy(behaviorAssetFile, behaviorAssetGenPath, name)
    copy(assetEditorFile, assetEditorGenPath, name)
    copy(assetForJsonFile, assetForJsonPath, name)
    print('Files generated.')

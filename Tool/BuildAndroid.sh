cd `dirname $0`

cd ..
UNITY_PROJECT_PATH=`pwd`

/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -quit -projectPath "${UNITY_PROJECT_PATH}" -executeMethod BatchBuild.AndroidBuild


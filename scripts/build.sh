#!/bin/bash
BASE_DIR=$(pwd)
OUTPUT_DIR=$BASE_DIR/outputs
TARGETS=("win-x86" "win-x64" "linux-arm" "linux-arm64" "linux-x64")

# Prepare
mkdir $OUTPUT_DIR
echo "Output directory: $OUTPUT_DIR"

# Build
dotnet restore

for TARGET in ${TARGETS[@]}; do
  echo "Build for $TARGET"
  TARGET_OUTPUT_DIR=$OUTPUT_DIR/$TARGET
  mkdir $TARGET_OUTPUT_DIR
  dotnet publish -r $TARGET -o $TARGET_OUTPUT_DIR -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true --self-contained

  if [[ $TARGET == win* ]];
  then
    POSTFIX=".exe"
  else
    POSTFIX=""
  fi
  mv $TARGET_OUTPUT_DIR/* $TARGET_OUTPUT_DIR/PowerRename-$TARGET$POSTFIX
done




# PowerRename
用来给文件批量重命名的小工具，使用.NET开发

## 用法
首先你需要一个xlsx文件，里面记录着源文件名与目标文件名的对应关系，格式如下
```
OldFilename NewFilename
oldfile1 newfile1
oldfile2 newfile2
...      ...
```
然后把所需要改名的文件装进一个文件夹里，最后以这样的形式使用：
```
Windows：
PowerRename.exe <你的xlsx的路径> <源文件夹路径> <输出文件夹路径（可选）>
其他平台：
PowerRename <你的xlsx的路径> <源文件夹路径> <输出文件夹路径（可选）>
```
如不填写输出文件夹，则会输出到与源文件夹平级的"output"文件夹
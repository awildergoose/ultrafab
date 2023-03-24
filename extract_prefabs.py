appended = ""

with open("manifest.txt", "r") as f:
    lines = f.read().split("\n")
    f.close()
    
    for line in lines:
        if ".prefab" in line:
            appended += line[2:] + "\n"

with open("prefabs.txt", "w") as f:
    f.write(appended)
    f.close()

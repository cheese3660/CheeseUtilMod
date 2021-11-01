import os
folder_in = "./lib"
template =  """<Reference Include="{name}">
<SpecificVersion>False</SpecificVersion>
<HintPath>lib\{name}.dll</HintPath>
</Reference>
"""
file_out = "references.txt"
with open(file_out,"w") as reflist:
    for filename in os.listdir(folder_in):
        if filename.endswith(".dll"):
            reflist.write(template.format(
                name = os.path.splitext(filename)[0]
            ))
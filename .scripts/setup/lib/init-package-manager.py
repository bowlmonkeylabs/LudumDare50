import os
import json

# function to add to JSON
def write_json(source_file, target_file):

    # Load data from source
    with open(source_file, 'r+') as file:
        source_date = json.load(file)

    # Update target with source
    with open(target_file, 'r+') as file:
        target_data = json.load(file)
        target_data["dependencies"].update(source_date)
        file.seek(0)
        json.dump(target_data, file, indent=2)



if __name__ == '__main__':
    # Get directory of this .py file
    dir = os.path.dirname(__file__)

    # Append to manifest-append.json
    manifest_source = os.path.join(dir, '..\\init-repos-config\\manifest-append.json');
    manifest_target = os.path.join(dir, '..\\..\\..\\Packages\\manifest.json')
    write_json(manifest_source, manifest_target)

    # Append to packages-lock.json
    packages_lock_source = os.path.join(dir, '..\\init-repos-config\\packages-lock-append.json');
    packages_lock_target = os.path.join(dir, '..\\..\\..\\Packages\\packages-lock.json')
    write_json(packages_lock_source, packages_lock_target)


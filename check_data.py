import json

with open('../data.json', encoding='utf-8') as fp:
    lines = fp.readlines()

lines = [json.loads(line) for line in lines]

category = set()

print(lines[0])

for line in lines:
    for entity in line['entities']:
        category.add(entity['category'])

print(category)
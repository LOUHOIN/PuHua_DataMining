import torch
from transformers import BertTokenizer
from label import category_name

net_path = './net_save/net_cpu_best.pth'

model = torch.load(net_path)
model.eval()

tokenizer = BertTokenizer.from_pretrained('bert-base-chinese')


def getinfo(token, label) -> list:
    res = []
    n = len(label[0])
    ptr = 0
    while ptr < n:
        val = label[0][ptr]
        if val % 2:
            start_idx = ptr
            while ptr + 1 < n and label[0][ptr + 1] == val + 1:
                ptr += 1
            res.append((category_name[(val - 1) >> 1], tokenizer.decode(token[0, start_idx:ptr + 1])))
        ptr += 1
    return res


if __name__ == '__main__':
    with torch.no_grad():
        while True:
            text = input()
            tokens = tokenizer.encode_plus(text, add_special_tokens=True, return_tensors='pt')
            pre = model(input_ids=tokens['input_ids'], attention_mask=tokens['attention_mask'])
            print('pre_label:', pre)
            privacy_word = getinfo(tokens['input_ids'], pre)
            print('privacy_word_list:', privacy_word)

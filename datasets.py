import torch
import json
from torch.utils.data import Dataset, DataLoader
from transformers import BertTokenizer, BertModel
from label import category_to_index



class TextDataset(Dataset):
    def __init__(self, file_path):
        self.data = []
        with open(file_path, 'r', encoding='utf-8') as f:
            for line in f:
                sample = json.loads(line)
                self.data.append(sample)

        # print(self.data)

        # Bert
        self.tokenizer = BertTokenizer.from_pretrained('bert-base-chinese')

    def __len__(self):
        return len(self.data)

    def __getitem__(self, idx):
        sample = self.data[idx]
        text = sample['text']
        entities = sample['entities']

        # get token
        # use tokenizer.encode_plus to get both input_ids and attention_mask
        # attention_mask is needed for BERT model to ignore padding tokens
        # return_tensors to 'pt' to get tensors directly
        text_token = self.tokenizer.encode_plus(text, add_special_tokens=True, return_tensors='pt')

        text_tokens = text_token['input_ids']
        input_list = text_tokens.numpy().tolist()[0]
        attention_mask = text_token['attention_mask']

        labels = torch.zeros(text_tokens.shape)

        # print(input_list)

        for entity in entities:
            category = entity['category']
            word = entity['privacy']
            # use tokenizer to get the position of the word in the tokenized sentence
            word_token = self.tokenizer.encode(word, add_special_tokens=False)
            pos_b, pos_e = get_pos(input_list, word_token)

            # set B-index I-index
            labels[0, pos_b] = category_to_index['B-'+category]  # B-index
            labels[0, pos_b + 1: pos_e + 1] = category_to_index['I-'+category]  # I-index

        labels = labels.long()
        # print(labels)
        return text_tokens, attention_mask, labels


def get_pos(arr, sub):
    arr_ptr = 0
    n = len(arr)
    m = len(sub)
    while arr_ptr < n:
        sub_ptr = 0
        arr_t_ptr = arr_ptr
        while sub_ptr < m and arr[arr_t_ptr] == sub[sub_ptr]:
            sub_ptr += 1
            arr_t_ptr += 1
        if sub_ptr == m:
            return arr_ptr, arr_ptr + m - 1
        arr_ptr += 1
    return -1, -1


if __name__ == '__main__':
    dataset = TextDataset(file_path='../data.json')
    for text, attention_mask, labels in dataset:
        print(text)
        print(dataset.tokenizer.decode(text[0]))
        print(labels)

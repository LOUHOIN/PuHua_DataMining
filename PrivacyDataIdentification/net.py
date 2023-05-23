import torch.nn as nn
from transformers import BertModel
from torchcrf import CRF
from datasets import TextDataset
from label import num_tags


class BertCRF(nn.Module):
    def __init__(self, bert_model, num_tags):
        super(BertCRF, self).__init__()
        self.bert = bert_model
        # random dropout
        self.dropout = nn.Dropout(0.1)
        self.linear = nn.Linear(bert_model.config.hidden_size, num_tags)
        self.crf = CRF(num_tags, batch_first=True)

    def forward(self, input_ids, attention_mask, labels=None):
        # input_ids: tokens (batch_size, seq_len)
        # attention_mask: (batch_size, seq_len)
        # labels: (batch_size, seq_len)

        outputs = self.bert(input_ids, attention_mask=attention_mask)
        sequence_output = outputs[0]  # (batch_size, seq_len, hidden_size)

        # dropout
        sequence_output = self.dropout(sequence_output)

        # reflect
        emissions = self.linear(sequence_output)  # (batch_size, seq_len, num_tags)

        # get pre
        mask = attention_mask.byte()
        output = self.crf.decode(emissions, mask=mask)

        if labels is not None:
            # calculate loss
            loss = -self.crf(emissions, labels, mask=mask)
            return loss, output
        else:
            # only return pre
            return output


if __name__ == '__main__':
    dataset = TextDataset(file_path='../data.json')
    net = BertCRF(bert_model=BertModel.from_pretrained('bert-base-chinese'), num_tags=num_tags)
    for text, attention_mask, labels in dataset:
        # print(text)
        if len(text[0]) <= 512:
            print(dataset.tokenizer.decode(text[0]))
            print(net(text, attention_mask))
            print(net(text, attention_mask, labels))

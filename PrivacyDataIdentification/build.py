import torch
import torch.optim as optim
from transformers import BertModel
from net import BertCRF
from datasets import TextDataset
from label import num_tags
from eprogress import LineProgress

num_epochs = 10
learning_rate = 0.000001
device = 'cuda'

train_dataset = TextDataset('../data.json')

bert_model = BertModel.from_pretrained('bert-base-chinese').to(device)

# select model
# model = BertCRF(num_tags=num_tags, bert_model=bert_model).to(device)
model = torch.load('./net_save/net_in_5_epoch.pth')

optimizer = optim.Adam(model.parameters(), lr=learning_rate)


def train(model, dataset, optimizer, time):
    model.train()

    total_loss = 0.0
    total_correct = 0
    total_place = 0

    lp = LineProgress(title="train_epoch_{}".format(time))

    dataset_len = len(dataset)

    for idx in range(0, dataset_len):
        input_ids, attention_mask, labels = dataset[idx]

        if len(input_ids[0]) > 512:
            lp.update((idx + 1) * 100 / dataset_len)
            continue

        input_ids = input_ids.to(device)
        labels = labels.to(device)
        attention_mask = attention_mask.to(device)

        loss, pre = model(input_ids=input_ids, attention_mask=attention_mask, labels=labels)

        loss.backward()

        # if idx % 50 == 0:
        #     print('\n{}_data:'.format(idx))
        #     print(labels)
        #     print(pre)
        #     print(loss)
        #     print()

        total_correct += (torch.tensor(pre, device=device) == labels).sum()

        total_place += len(labels[0])

        optimizer.step()
        optimizer.zero_grad()

        total_loss += loss.item()
        lp.update((idx + 1) * 100 / dataset_len)

    avg_loss = total_loss / dataset_len
    avg_acc = total_correct / total_place

    return avg_loss, avg_acc


def test(model, dataset, time):
    model.eval()

    total_correct = 0
    total_place = 0

    lp = LineProgress(title="test_epoch_{}".format(time))

    dataset_len = len(dataset)

    with torch.no_grad():
        for idx in range(0, dataset_len):
            input_ids, attention_mask, labels = dataset[idx]

            if len(input_ids[0]) > 512:
                lp.update((idx + 1) * 100 / dataset_len)
                continue

            input_ids = input_ids.to(device)
            labels = labels.to(device)
            attention_mask = attention_mask.to(device)

            pre = model(input_ids=input_ids, attention_mask=attention_mask, labels=None)

            if idx % 100 == 0:
                print('\n{}_data:'.format(idx))
                print(labels)
                print(pre)
                print()

            total_correct += (torch.tensor(pre, device=device) == labels).sum()

            total_place += len(labels[0])
            lp.update((idx + 1) * 100 / dataset_len)

    avg_acc = total_correct / total_place

    return avg_acc


if __name__ == '__main__':
    for epoch in range(num_epochs):
        train_loss, train_acc = train(model=model, dataset=train_dataset, optimizer=optimizer, time=epoch + 1)
        print(f' Train Loss: {train_loss:.4f}, Train Acc: {train_acc * 100:.4f}%')
        torch.save(model, './net_save/net_in_{}_epoch.pth'.format(epoch + 1))
        print("Net Save Successful.")
        test_acc = test(model=model, dataset=train_dataset, time=epoch + 1)
        print(f' Test Acc: {test_acc * 100:.4f}%')

import torch

net_path = './net_save/net_in_10_epoch.pth'
write_cpu_pth = './net_save/net_cpu_best.pth'

device = torch.device('cpu')
model = torch.load(net_path, map_location=device)
torch.save(model, write_cpu_pth)

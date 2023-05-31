# 隐私数据识别模块

Privacy Data Identification

## 概述

本模块将文本向量化后，使用BERT预训练的语言表征模型对输入语句进行编码，得到每个token的隐藏向量。然后使用线性层将隐藏向量映射到对应的标签空间，得到每个token的标签分数。最后利用CRF条件随机场对标签分数进行解码，得到最优的标签序列。 BERT具有双向预测的能力，分析文本时不仅仅考虑左侧的token，还兼顾了右侧的token，能更好地根据上下文推测词义，而CRF条件随机场也是根据前后标签之间的转移概率推测当前的标签。模型的损失函数由CRF的当前转移状态矩阵在正确路径的分数与所有路径分数之和的对数似然差给出，通过梯度下降法来优化权重。

BERT+Linear+CRF

## 训练

* 数据集

    CCF大数据与计算智能大赛-非结构化商业文本信息中隐私信息识别

* 架构

    BERT->Linear->CRF

    loss由CRF的对数似然差给出

    学习率维持0.000001不变

## 使用

* 训练模型导入

* 运行

        demo.py

* 输入对应的文本

## 仓库结构

    ├─build.py
    ├─check_data.py
    ├─datasets.py
    ├─demo.py
    ├─label.py
    ├─net.py
    └─net_to_cpu.py

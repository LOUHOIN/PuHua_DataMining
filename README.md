# PuHua_DataMining
## 普华数据挖掘
- **赛题简介:** 在企业的运营中会产生大量的数据，这些数据也会在不同的业务场景中被传递、使用。但是由于不同场景的数据保护能力、数据暴露面不同，造成数据在流通使用的过程中存在着不同的泄露风险。因此，如何高效、准确地识别数据且分类分级，成为了数据安全保护的基础，将直接决定数据安全工作的成功与否。
- **赛题相关:** 数据分级分类、正则表达式、数据库基础、网络通讯协议基本原理、软件开发基本概念


## 数据安全&隐私数据（研究背景）
- 在企业运营中会产生大量的数据，而数据安全在数字领域一直是个备受关注的问题。在数据安全的生命周期中，数据的传输阶段和共享阶段往往是是发生数据安全事故的重灾区。对于企业来说，如何基于已有的数据业务，解决数据传输流转过程中的一列问题以及如何高效、准确地识别隐私数据且分类分级不仅是崭新的研究方向，也是数据安全保护的基础。

## 存在的问题
- **业务过程**：在业务过程中，场景复杂性和用户多样性是非常常见的情况。例如用户在不同的操作环境进行业务操作时，不同场景存在相关的安全问题
- **数据流转**：在数据流转中，会存在数据质量、安全性、效率、集成和可扩展等问题。例如数据质量问题会影响数据分析，效率问题会影响业务流程的稳定性
- **隐私数据识别**：隐私数据识别的准确性影响整个业务的数据流转的安全性。数据流转的数据处理是否高效，取决于能否对隐私数据进行精准标记。
- **架构设计**：随着系统和应用不断扩展和升级，系统复杂性也不断提高。如业务流程、技术框架、可扩展性以及应用安全都要在设计架构的时候考虑到

## 解决技术
在以上存在的问题的基础上，经过检索和选择，最终确定了解决方案和技术，并合理地设计了整个业务流程，以有效地解决问题
- IdentityServer4：Claims、JWT Token、OpenID Connect
- 负载均衡：Request Server、CDN、Message Queue
- 隐私数据识别：Bidirectional Encoder Representation from Transformers
、Conditional Random Field
- 读写分离：MySQL Proxy、Redis Cache


# 相关说明文档
授权认证模块说明： [IdentityServer4->README.md](https://github.com/LOUHOIN/PuHua_DataMining/tree/main/IdentityServer4)<p>
负载均衡模块说明： [LoadBalance->README.md](https://github.com/LOUHOIN/PuHua_DataMining/tree/main/LoadBalance)<p>
隐私数据识别模块说明： [PrivacyDataIdentification->README.md](https://github.com/LOUHOIN/PuHua_DataMining/tree/main/PrivacyDataIdentification)<p>
读写分离模块说明： [Read_WriteSplitting->README.md](https://github.com/LOUHOIN/PuHua_DataMining/tree/main/Read_WriteSplitting)<p>
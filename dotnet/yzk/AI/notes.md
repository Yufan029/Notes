## chapter 5 本地部署大模型及AI的原理 ##
- 要有很好的GPU，或者GPU集群
1. docker run -d --name ollama -p 11434:11434 ollama/ollama
2. docker exec -it ollama ollama run llama3
3. docker exec -it ollama ollama run mxbai-embed-large
4. api base url: http://127.0.0.1:11434/v1

- 本地部署无需 apiKey = "";

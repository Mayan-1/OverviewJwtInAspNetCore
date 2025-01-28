# JWT - JSON Web Token

## O que é JSON Web Token?

JSON Web Token (JWT) é um padrão aberto que define uma maneira compacta e independente de transmitir informações entre partes utilizando JSON. A informação pode ser verificada e confiável, pois ela é digitalmente assinada. JWTs podem ser assinado usando um segredo (com o algoritmo HMAC) ou pares de chaves públicas/privadas usando RSA ou ECDSA.

## Quando você deve usar JSON Web Tokens?

**Autorização:** Essa é a maneira mais comum de utilizar JWT. Uma vez que o usuário está logado, cada request subsequente irá incluir o JWT, permitindo que o usuário acesse rotas, serviços ou recurso que são permitidos com aquele token.

**Troca de informações:** JWTs são uma maneira segura de transmitir informações entre partes. Porque JWTs podem ser assinados, por exemplo utilizando pares de chave pública/privada, podendo confirmar que o remetente é realmente quem diz ser. Adicionalmente, como a assinatura é calculada usando o header e o payload, você pode verificar se o conteúdo não foi adulterado.

## **Qual a estrutura do JSON Web Token?**

De forma compacta, JSON Web Tokens consistem em três partes separadas por um ponto, são elas:

- Header
- Payload
- Signature

### Header

O header tipicamente consiste em duas partes: o tipo do token, que no caso é JWT, e o algoritmo de assinatura utilzado, como HMAC SHA256 ou RSA.

Por exemplo:

```json
{
  "alg": "HMAC",
  "typ": "JWT"
}
```

### Payload

A segunda parte do token é o payload, que contém as claims. Claims são declarações sobre uma entidade e dados adicionais. Há três tipos de claims: registradas, públicas ou privadas.

- **Claims registradas:** Esses são um conjunto de declarações predefinidas que não são obrigatórias, mas recomendadas, para fornecer um conjunto de declarações úteis e interoperáveis. Algumas delas são: **iss** (emissor), **exp** (tempo de expiração), **sub** (assunto), **aud** (audiência) e outras.
- **Claims públicas:** Essas podem ser definidas à vontade por aqueles que utilizam JWTs. No entanto, para evitar colisões, elas devem ser definidas no Registro de JSON Web Token da IANA ou como um URI que contenha um namespace resistente a colisões.
- **Claims privadas:** Essas são as declarações personalizadas criadas para compartilhar informações entre partes que concordam em usá-las e que não são declarações registradas nem públicas.

A seguir o exemplo de um payload:

```json
{
  "sub": "1234567890",
  "name": "John Doe",
  "admin": true
}
```

> Note que, para tokens assinados, essas informações embora protegidas contra adulterações, ainda pode ser lida por qualquer um. Então, não coloque informação secreta no payload ou header de um JWT a menos que ela esteja criptografada.

### Signature

Para criar a parte de signature você deve pegar o header codificado, o payload codificado, um segredo, o algoritmo especificado no header, e assinar isso.

Por exemplo, se você quer usar o algoritmo HMAC SHA256, a assinatura seria criada da seguinte maneira:

```json
HMACSHA256(
	base64UrlEncode(header) + "." +
	base64UrlEncode(payload),
	secret)
```

A assinatura é utilizada para verificar se a mensagem não foi alterada durante o caminho, e, no caso de tokens assinados, se o remetente do JWT é realmente quem diz ser.

## **Como JSON Web Tokens funcionam?**

Em autenticação, quando o usuário faz o login com sucesso usando suas credenciais, um JSON Web Token será retornado. Como os tokens são credenciais, é necessário ter muito cuidado para evitar problemas de segurança.

Sempre que o usuário quiser acessar uma rota ou recurso protegido, ele deve enviar o JWT, normalmente no header de autorização usando o esquema Bearer. O conteúdo do header deve se parecer com o seguinte:

```json
Authorization: Bearer <token>
```

Isso pode ser, em certos casos, um mecanismo de autorização sem estado. As rotas protegidas do servidor verificarão um JWT válido no cabeçalho **Authorization** e, se ele estiver presente, o usuário será autorizado a acessar os recursos protegidos. Se o JWT contiver os dados necessários, a necessidade de consultar o banco de dados para certas operações pode ser reduzida, embora isso nem sempre seja o caso.

Observe que, se você enviar tokens JWT por meio de cabeçalhos HTTP, deve tentar evitar que eles fiquem muito grandes. Alguns servidores não aceitam mais de 8 KB em cabeçalhos. Se você estiver tentando incluir muitas informações em um token JWT, como todas as permissões de um usuário, pode ser necessário buscar uma solução alternativa, como o **Auth0 Fine-Grained Authorization**.

O seguinte diagram mostra como o JWT é obtido e utilizado para acessar APIs ou recursos:

![image.png](https://cdn.auth0.com/website/jwt/introduction/client-credentials-grant.png)

1. A aplicação ou cliente solicita autorização ao servidor de autorização. Isso é feito por meio de um dos diferentes fluxos de autorização. Por exemplo, uma aplicação web típica compatível com OpenID Connect passará pelo endpoint **/oauth/authorize** utilizando o fluxo de código de autorização (**authorization code flow**).
2. Quando a autorização é concedida, o servidor de autorização retorna um **token de acesso** para a aplicação.
3. A aplicação usa o **token de acesso** para acessar um recurso protegido (como uma API).

## Por que devemos usar JSON Web Tokens?

Como JSON é menos verboso que XML, quando codificado, seu tamanho também é menor, tornando o JWT mais compacto do que o SAML. Isso faz do JWT uma boa escolha para ser transmitido em ambientes HTML e HTTP.

Em termos de segurança, o **SWT** só pode ser assinado de forma simétrica por um segredo compartilhado utilizando o algoritmo HMAC. No entanto, tokens JWT e SAML podem usar um par de chaves pública/privada no formato de um certificado X.509 para assinatura. Assinar XML com **XML Digital Signature** sem introduzir falhas de segurança obscuras é muito mais difícil em comparação com a simplicidade de assinar JSON.

Os analisadores JSON são comuns na maioria das linguagens de programação porque mapeiam diretamente para objetos. Por outro lado, o XML não possui um mapeamento natural de documento para objeto. Isso torna o JWT mais fácil de trabalhar em comparação com as afirmações SAML.

Em relação ao uso, o JWT é amplamente utilizado em escala na Internet. Isso destaca a facilidade de processamento do **JSON Web Token** no lado do cliente em múltiplas plataformas, especialmente em dispositivos móveis.

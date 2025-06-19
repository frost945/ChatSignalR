
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().catch(err => console.error(err.toString()));

//Отображает что подключился новый участник к чату
function joinChat()
{
    user = document.getElementById("username").value.trim();
    chat = document.getElementById("chatname").value.trim();

    console.log("Подключаемся к чату", user, chat);

    if (user && chat)
    {
        document.getElementById("login").style.display = "none";
        document.getElementById("chat").style.display = "block";
        document.getElementById("chat-title").innerText = `Чат: ${chat}`;

        // Отправляем данные пользователя на сервер
        connection.invoke("joinChat", { userName: user, chatName: chat })
            .catch(err => console.error(err.toString()));
    }
    else
    {
        alert("Введите имя и название чата");
    }
}

//Отправляем сообщение на сервер
function sendMessage() {
    console.log("Отправляем сообщение на сервер");

    const message = document.getElementById("messageInput").value.trim();
    if (message) {
        connection.invoke("SendMessage", { userName: user, chatName: chat }, message)
            .catch(err => console.error(err.toString()));

        document.getElementById("messageInput").value = "";
    }
}


//Получаем сообщение на клиенте
connection.on("ReceiveMessage", (userName, chatName, message, sentiment) =>
{
    console.log("Получаем сообщение на клиенте");
    //const chatName = userConnection.chatName;
    //const userName = userConnection.userName;

    if (chatName === chat) {
        const msg = document.createElement("div");
        msg.textContent = `${userName}: ${message}`;

        switch (sentiment) {
            case "Positive": msg.classList.add("positive-msg"); break;
            case "Negative": msg.classList.add("negative-msg"); break;
            case "Neutral":
            default: msg.classList.add("neutral-msg"); break;
        }

        document.getElementById("messages").appendChild(msg);
    }
});

        /*  connection.on("ReceiveHistory", function (chat, messages) {
        console.log("ReceiveHistory triggered!");
    console.log("Chat ID:", chat);
    console.log("Messages received:", messages);

    if (Array.isArray(messages)) {
        messages.reverse().forEach(msg => {
            displayMessage(msg.user, msg.message);
        });
              } else {
        console.warn("Received 'messages' is not an array:", messages);
              }
          });      

    let skip = 0;

    const chatBox = document.getElementById("messages");

    if (chatBox) {
        chatBox.addEventListener('scroll', function () {
            if (chatBox.scrollTop === 0) {
                skip += 50;
                connection.invoke("LoadChatHistory", chat, skip);
            }
        });
        } else {
        console.error("Элемент chatBox не найден в DOM.");
        }
        */

    




   // let user = "";
    //let chat = "";

    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();


    connection.on("ReceiveMessage", (userName, chatName, message, sentiment) => {
    console.log("ReceiveMessage on");
        if (chatName === chat)
        {
        const msg = document.createElement("div");
        msg.textContent = `${userName}: ${message}`;

            switch (sentiment)
         {
           case "Positive":
           msg.classList.add("positive-msg");
           break;
           case "Negative":
           msg.classList.add("negative-msg");
           break;
           case "Neutral":
           default:
           msg.classList.add("neutral-msg");
           break;
         }

        document.getElementById("messages").appendChild(msg);
        }
    });



    connection.start().catch(err => console.error(err.toString()));

    // отображаем сообщение что подключился новый участник к чату
    function joinChat() {

        user = document.getElementById("username").value.trim();
    chat = document.getElementById("chatname").value.trim();

    if (user && chat) {
        document.getElementById("login").style.display = "none";
    document.getElementById("chat").style.display = "block";
    document.getElementById("chat-title").innerText = `Чат: ${chat}`;

    // Отправляем данные пользователя на сервер
    connection.invoke("joinChat", {
        userName: user,
    chatName: chat
                }).catch(err => console.error(err.toString()));
            } else {
        alert("Введите имя и название чата");
            }
    console.log("Подключаемся к чату", user, chat);
        }

    //отображаем сообщения пользователей чата
    function sendMessage() {
            const message = document.getElementById("messageInput").value.trim();
    if (message) {
        connection.invoke("SendMessage", user, chat, message)
            .catch(err => console.error(err.toString()));
    document.getElementById("messageInput").value = "";
            }
        }


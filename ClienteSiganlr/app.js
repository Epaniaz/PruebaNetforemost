document.addEventListener("DOMContentLoaded", function () {
    // Configurar la conexión SignalR
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5107/usuariohub", { skipNegotiation: true, transport: signalR.HttpTransportType.WebSockets })
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Definir la función de recepción de mensajes
    connection.on("RecibeUsuario", (user) => {
        const body =  `<div>
                            <div>${user.login}</div>
                            <div>${user.identificativo}</div>
                            <div>${user.nombres} ${user.apellidos}</div>
                            <div>${user.correo}</div>
                            <div>${user.telefono}</div>
                            <img src="${user.avatar}" />
                        </div>`;

        alertify.alert()
            .setHeader("Advertencia")
            .setContent(body)
            .show();
    });

    // Iniciar la conexión
    connection.start()
        .then(() => {
            console.log("Conexión SignalR establecida.");
        })
        .catch((err) => {
            console.error(err);
        });

    // Manejar el envío de mensajes
    // document.getElementById("sendMessageBtn").addEventListener("click", function () {
    //     const user = document.getElementById("userInput").value;
    //     const message = document.getElementById("messageInput").value;

    //     // Enviar el mensaje al servidor
    //     connection.invoke("SendMessage", user, message)
    //         .catch((err) => {
    //             console.error(err.toString());
    //         });
    // });
});

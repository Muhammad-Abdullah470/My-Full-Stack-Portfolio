const backendUrl = "http://localhost:3000"; // Change if hosted

async function sendMessage() {
  const input = document.getElementById("userInput").value.trim();
  if (!input) return;

  appendMessage("user", input);
  document.getElementById("userInput").value = "";
  appendMessage("ai", "Thinking... 🤖");

  try {
    if (input.toLowerCase().includes("generate image of")) {
      const prompt = input.split("generate image of")[1].trim();
      const response = await fetch(`${backendUrl}/image`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ prompt })
      });
      const data = await response.json();
      updateLastAIMessage("Here's your image:");
      appendImage(data.url);
      speak("Here's your image.");
    } else {
      const response = await fetch(`${backendUrl}/chat`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ message: input })
      });
      const data = await response.json();
      updateLastAIMessage(data.answer);
      speak(data.answer);
    }
  } catch (err) {
    console.error(err);
    updateLastAIMessage("❌ Codelith AI failed to respond.");
  }
}

function appendMessage(sender, text) {
  const div = document.createElement("div");
  div.className = `message ${sender}`;
  div.innerText = `${sender === "user" ? "You" : "Codelith AI"}: ${text}`;
  document.getElementById("chatbox").appendChild(div);
  document.getElementById("chatbox").scrollTop = document.getElementById("chatbox").scrollHeight;
}

function updateLastAIMessage(text) {
  const messages = document.querySelectorAll(".message.ai");
  if (messages.length > 0) {
    messages[messages.length - 1].innerText = `Codelith AI: ${text}`;
  }
}

function appendImage(url) {
  const img = document.createElement("img");
  img.src = url;
  img.alt = "Generated Image";
  img.className = "generated-image";
  document.getElementById("chatbox").appendChild(img);
}

function startVoice() {
  const recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition)();
  recognition.lang = "en-US";
  recognition.start();
  recognition.onresult = function (event) {
    document.getElementById("userInput").value = event.results[0][0].transcript;
    sendMessage();
  };
}

function speak(text) {
  const msg = new SpeechSynthesisUtterance(text);
  msg.lang = "en-US";
  window.speechSynthesis.speak(msg);
}

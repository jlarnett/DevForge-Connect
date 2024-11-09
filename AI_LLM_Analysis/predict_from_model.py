import spacy

def spacy_prediction(text):
    nlp = spacy.load("trained_models")

    # Mapping intents to technologies
    # Mapping of categories to specific technologies
    category_to_tech_map = {
        "STORAGE": ["MySQL", "PostgreSQL", "MongoDB", "Amazon S3", "Redis"],
        "AI": ["TensorFlow", "PyTorch", "Scikit-learn", "OpenAI", "Keras", "artificial intelligence", "matchine learning",
               "MACHINE LEANRING", "Reinforcement Learning", "robot", "robotics"],
        "DATA_ANALYSIS": ["Pandas", "NumPy", "Tableau", "Jupyter Notebook", "Apache Spark"],
        "COMMUNICATION": ["WebSockets", "RESTful APIs", "Twilio", "RabbitMQ", "GraphQL", "TCPIP", "HTTPS" "SSL"],
        "WEB_DEVELOPMENT": ["React", "Django", "Flask", "Angular", "Vue.js", "HTML", "html", "javascript", "JAVASCRIPT"],
        "GAME_DEVELOPMENT": ["Unity", "Unreal Engine", "C#", "OpenGL", "Blender"],
        "MOBILE_DEVELOPMENT": ["Flutter", "React Native", "Swift", "Kotlin", "Firebase"],
        "IOT": ["MQTT", "Arduino", "Raspberry Pi", "AWS IoT Core", "Node-RED", "wifi", "2.4GHZ", "5GHZ", "5ghz", "bluetooth"],
        "NETWORK": ["Lan", "LAN", "local area network", "network", "DNS","firewall", "port", "ports", 
                    "protocol", "protocols", "server farm", "IP", "WAN", "FIREWALL", "DHCP", "Server", "tcp", "tcp/ip"],
        "OPERATING_SYSTEMS": ["WINDOWS", "LINUX", "KALI", "Kali", "windows", "linux", "kernal", "cpu scheduler", "Paging", "Virtual Memory", "c"],
        "REALTIME": ["Realtime", "Real-Time", "REAL-TIME", "EMBEDDED", "Embedded", "realtime", "c", "execution-speed", "quick response time"],
    }

    # Example usage after classification
    #text = "Develop a mobile game with cloud-based storage and AI-driven difficulty adjustments."
    doc = nlp(text)
    predicted_categories = [category for category, score in doc.cats.items() if score >= 0.5]
    print(predicted_categories)
    return predicted_categories

# Suggest technologies based on predicted categories
# suggested_technologies = []
# for category in predicted_categories:
#     suggested_technologies.extend(category_to_tech_map.get(category, []))

# print(f"Suggested Technologies: {suggested_technologies}")


# # Function to extract technologies based on intent
# def extract_technologies_based_on_intent(text):
#     doc = nlp(text)
#     intent = None
#     for label, score in doc.cats.items():
#         if score > 0.5:  # Set a threshold for the confidence score
#             intent = label
#             break
    
#     if intent:
#         technologies = category_to_tech_map.get(intent, [])
#         return intent, technologies
#     else:
#         return "Unknown", []

# # Example usage
# text = "The app should be able to control smart home devices and provide voice communication."
# intent, technologies = extract_technologies_based_on_intent(text)
# print(f"Intent: {intent}")
# print(f"Suggested Technologies: {technologies}")
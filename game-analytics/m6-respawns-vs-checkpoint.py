import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv("new-raw-analytics.csv")
checkpoint_events = df[df['eventType'] == 'checkpointUsed']

checkpoint_usage = checkpoint_events.groupby(['currentLevel', 'Checkpoint']).size().reset_index(name='count')

# Plotting
for level in sorted(checkpoint_usage['currentLevel'].unique()):
    level_data = checkpoint_usage[checkpoint_usage['currentLevel'] == level]
    plt.figure(figsize=(10, 6))
    plt.bar(level_data['Checkpoint'], level_data['count'], color='blue')
    plt.title(f"Respawns at each checkpoint at Level {level}")
    plt.xlabel("Checkpoint")
    plt.ylabel("Number of Times Used")
    plt.xticks(rotation=45)  # Rotates the x-axis labels to prevent overlap
    plt.grid(True)
    plt.show()

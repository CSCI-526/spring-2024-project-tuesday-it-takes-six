import pandas as pd
import matplotlib.pyplot as plt


df = pd.read_csv("new-raw-analytics.csv")
df['currentLevel'] = pd.to_numeric(df['currentLevel'], errors='coerce').dropna().astype(int) #type conversion to deal with errors. 

# Filter to include only timeSwitch events and checkpoints
time_switches = df[df['eventType'] == 'timeSwitch']
checkpoint_counts = time_switches.groupby(['currentLevel', 'Checkpoint']).size().reset_index(name='count')

# Plotting
for level in sorted(checkpoint_counts['currentLevel'].unique()):
    level_data = checkpoint_counts[checkpoint_counts['currentLevel'] == level]
    plt.figure(figsize=(10, 6))
    plt.bar(level_data['Checkpoint'], level_data['count'], color='blue')
    plt.title(f"Sum of TimeSwitch Events by Checkpoint at Level {level}")
    plt.xlabel("Checkpoint")
    plt.ylabel("Sum of TimeSwitch Events")
    plt.xticks(rotation=45)  # Rotates the x-axis labels to prevent overlap
    plt.grid(True)
    plt.show()

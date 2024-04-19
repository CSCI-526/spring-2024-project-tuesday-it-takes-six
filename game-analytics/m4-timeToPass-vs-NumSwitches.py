import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv("new-raw-analytics.csv")
df = df[pd.to_numeric(df['currentLevel'], errors='coerce').notna()]  # This will drop any row with non-numeric 'currentLevel'
df['currentLevel'] = pd.to_numeric(df['currentLevel']).astype(int)  # Convert to integer

level_completed = df[df['eventType'] == 'levelCompleted']
time_switches = df[df['eventType'] == 'timeSwitch']
results = {level: [] for level in range(1, 6)}

for level in range(1, 6):
    completed_sessions = level_completed[level_completed['currentLevel'] == level]
    
    for session_id in completed_sessions['sessionID'].unique():
        time_at_completion = completed_sessions[completed_sessions['sessionID'] == session_id]['timePassedSinceGameStart'].iloc[0]
        time_switch_count = time_switches[(time_switches['sessionID'] == session_id) & (time_switches['currentLevel'] == level)].shape[0]
        results[level].append((time_at_completion, time_switch_count/6))

# Plotting
for level in range(1, 6):
    if results[level]:
        times, counts = zip(*results[level])
        plt.figure(figsize=(10, 6))
        plt.scatter(times, counts)
        plt.title(f"Level {level} Completion Times vs. Time Switch Events")
        plt.xlabel("Time Passed Since Game Start")
        plt.ylabel("Number of Time Switch Events")
        plt.grid(True)
        plt.show()

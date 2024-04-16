import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

df = pd.read_csv("analytics-raw-csv.csv")
df['Timestamp'] = pd.to_datetime(df['Timestamp'])


def calculate_time_spent(group):
    # Assume starting in the "present" dimension
    current_dim = 'present'
    start_time = group.iloc[0]['Timestamp']
    durations = {'past': pd.Timedelta(0), 'present': pd.Timedelta(0)}
    
    for i, row in group.iterrows():
        if row['eventType'] == 'timeSwitch':
            time_spent = row['Timestamp'] - start_time
            durations[current_dim] += time_spent
            
            # Toggle dimension
            current_dim = 'past' if current_dim == 'present' else 'present'
            start_time = row['Timestamp']
    
    # Add time from last switch 
    if start_time != group.iloc[-1]['Timestamp']:
        durations[current_dim] += group.iloc[-1]['Timestamp'] - start_time
    
    return pd.Series(durations, index=['past', 'present'])

dimension_time = df.groupby(['currentLevel', 'sessionID']).apply(calculate_time_spent)
dimension_time = dimension_time.div(pd.Timedelta(minutes=1))
total_time_per_level = dimension_time.groupby('currentLevel').sum()
percentage_time_per_level = dimension_time.groupby('currentLevel').sum().apply(lambda x: x / total_time_per_level.sum(axis=1) * 100)

# Plotting 
fig, ax = plt.subplots(figsize=(14, 8))
width = 0.35  # the width of the bars
levels = percentage_time_per_level.index
ind = np.arange(len(levels))  # the x locations for the groups

rects1 = ax.bar(ind - width/2, percentage_time_per_level['past'], width, label='Past')
rects2 = ax.bar(ind + width/2, percentage_time_per_level['present'], width, label='Present')

# Add some text for labels, title, and custom x-axis tick labels, etc.
ax.set_xlabel('Level')
ax.set_ylabel('Percentage of Time Spent (%)')
ax.set_title('Percentage of Time Spent in Dimensions Across All Levels')
ax.set_xticks(ind)
ax.set_xticklabels([f'Level {level}' for level in levels])
ax.legend()

plt.grid(True)
plt.show()

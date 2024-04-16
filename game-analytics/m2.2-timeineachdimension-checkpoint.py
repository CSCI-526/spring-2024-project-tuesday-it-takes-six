import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

df = pd.read_csv("analytics-raw-csv.csv")

# Data verification to silence errors
expected_columns = ['Timestamp', 'sessionID', 'eventType', 'Checkpoint', 'currentLevel']
missing_columns = [col for col in expected_columns if col not in df.columns]
if missing_columns:
    raise ValueError(f"Missing columns in the DataFrame: {missing_columns}")

df['Timestamp'] = pd.to_datetime(df['Timestamp'])
df = df.sort_values(['sessionID', 'Timestamp'])

def calculate_time_spent(group):
    current_dim = 'present'
    start_time = group.iloc[0]['Timestamp']
    durations = []
    
    for i, row in group.iterrows():
        if row['eventType'] == 'timeSwitch':
            time_spent = row['Timestamp'] - start_time
            durations.append((current_dim, row['Checkpoint'], time_spent))
            current_dim = 'past' if current_dim == 'present' else 'present'
            start_time = row['Timestamp']


    if start_time != group.iloc[-1]['Timestamp']:
        durations.append((current_dim, group.iloc[-1]['Checkpoint'], group.iloc[-1]['Timestamp'] - start_time))
    
    return pd.DataFrame(durations, columns=['Dimension', 'Checkpoint', 'Duration'])

dimension_time = df.groupby(['currentLevel', 'sessionID']).apply(calculate_time_spent).reset_index(drop=True)
dimension_time['Duration'] = dimension_time['Duration'].dt.total_seconds() / 60
time_per_checkpoint_level = dimension_time.groupby(['currentLevel', 'Checkpoint', 'Dimension']).sum().unstack().fillna(0)

# Plotting 
for level, level_data in time_per_checkpoint_level.groupby(level=0):
    fig, ax = plt.subplots(figsize=(14, 8))
    checkpoints = level_data.index
    ind = np.arange(len(checkpoints))  # the x locations for the groups
    width = 0.35  # the width of the bars

    rects1 = ax.bar(ind - width/2, level_data[('Duration', 'past')], width, label='Past')
    rects2 = ax.bar(ind + width/2, level_data[('Duration', 'present')], width, label='Present')

    # Add some text for labels, title, and custom x-axis tick labels, etc.
    ax.set_xlabel('Checkpoint')
    ax.set_ylabel('Time Spent (minutes)')
    ax.set_title(f'Time Spent in Dimensions for Level {level}')
    ax.set_xticks(ind)
    ax.set_xticklabels(checkpoints)
    ax.legend()

    plt.grid(True)
    plt.show()
